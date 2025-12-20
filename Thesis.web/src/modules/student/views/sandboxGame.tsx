import React, { useState, useEffect, useCallback } from "react";
import styles from "../styles/sandboxGame.module.css";
import modalStyles from "../../exercise/styles/exerciseModal.module.css";
import { toast } from "react-toastify";
import type {
  ExerciseDetails,
  AnswerModel,
} from "../../exercise/ExerciseService";
import GameService, { type GameDetails } from "../gameService";
import ExerciseService from "../../exercise/ExerciseService";

export default function SandboxGame() {
  const [sessionId, setSessionId] = useState<string | null>(null);
  const [currentExercise, setCurrentExercise] =
    useState<ExerciseDetails | null>(null);
  const [gameDetails, setGameDetails] = useState<GameDetails | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isGameOver, setIsGameOver] = useState(false);

  const [userAnswer, setUserAnswer] = useState<string>("");
  const [userNumericAnswer, setUserNumericAnswer] = useState<string>("");
  const [selectedOption, setSelectedOption] = useState<string>("");
  const [shuffledOptions, setShuffledOptions] = useState<string[]>([]);

  const [timeLeft, setTimeLeft] = useState<number>(0);

  useEffect(() => {
    let timer: NodeJS.Timeout;
    if (sessionId && timeLeft > 0 && !isGameOver) {
      timer = setInterval(() => {
        setTimeLeft((prev) => prev - 1);
      }, 1000);
    } else if (sessionId && timeLeft === 0 && !isGameOver) {
      finishGame(sessionId);
    }

    return () => clearInterval(timer);
  }, [sessionId, timeLeft, isGameOver]);

  const startGame = async () => {
    setIsLoading(true);
    setIsGameOver(false);
    setGameDetails(null);
    try {
      const newSessionId = await GameService.StartSession();
      setSessionId(newSessionId);
      setTimeLeft(60);
      await fetchNextQuestion(newSessionId);
    } catch (error) {
      toast.error("Nie udało się rozpocząć gry.");
    } finally {
      setIsLoading(false);
    }
  };

  const fetchNextQuestion = async (sid: string) => {
    try {
      const exercise = await GameService.GetQuestion(sid);
      setCurrentExercise(exercise);
      resetAnswerFields(exercise);
    } catch (error: any) {
      if (error.response?.status === 404) {
        finishGame(sid);
      } else {
        toast.error("Błąd podczas pobierania pytania.");
      }
    }
  };

  const resetAnswerFields = (exercise: ExerciseDetails) => {
    setUserAnswer("");
    setUserNumericAnswer("");
    setSelectedOption("");

    if (exercise.answer?.correctOption) {
      const options = [
        exercise.answer.correctOption,
        exercise.answer.incorrectOption1,
        exercise.answer.incorrectOption2,
        exercise.answer.incorrectOption3,
      ].filter((opt) => opt && opt.trim() !== "");
      setShuffledOptions([...options].sort(() => Math.random() - 0.5));
    } else {
      setShuffledOptions([]);
    }
  };

  const finishGame = async (sid: string) => {
    try {
      const details = await GameService.GetGameDetails(sid);
      setGameDetails(details);
      setIsGameOver(true);
      setCurrentExercise(null);
      setTimeLeft(0);
    } catch (error) {
      console.error("Błąd pobierania podsumowania gry");
    }
  };

  const handleSubmitAnswer = async () => {
    if (!currentExercise || !sessionId) return;

    const numericValue =
      userNumericAnswer !== "" ? Number(userNumericAnswer) : null;
    const answer: AnswerModel = {
      correctText: userAnswer,
      correctOption: selectedOption,
      correctNumber: numericValue,
    };

    try {
      await ExerciseService.GameAnswer(
        currentExercise.publicId,
        sessionId,
        answer
      );

      toast.success("Poprawna odpowiedź!");

      await fetchNextQuestion(sessionId);
    } catch (error: any) {
      if (error.response?.status === 404) {
        finishGame(sessionId);
      } else if (error.response?.status === 422) {
        toast.error("Błędna odpowiedź!.");
        await fetchNextQuestion(sessionId);
      }
    }
  };

  const formatTime = (seconds: number) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins}:${secs < 10 ? "0" : ""}${secs}`;
  };

  const renderAnswerSection = () => {
    if (!currentExercise?.answer) return null;
    const { answer } = currentExercise;

    if (answer.correctText) {
      return (
        <textarea
          className={modalStyles.textarea}
          value={userAnswer}
          onChange={(e) => setUserAnswer(e.target.value)}
          placeholder="Wpisz odpowiedź..."
        />
      );
    }

    if (
      answer.correctNumber !== undefined &&
      answer.correctNumber !== null &&
      answer.correctNumber !== 0
    ) {
      return (
        <input
          type="number"
          className={modalStyles.numberInput}
          value={userNumericAnswer}
          onChange={(e) => setUserNumericAnswer(e.target.value)}
          placeholder="Wpisz liczbę..."
        />
      );
    }

    if (shuffledOptions.length > 0) {
      return (
        <div className={modalStyles.optionsGrid}>
          {shuffledOptions.map((option, idx) => (
            <button
              key={idx}
              className={`${modalStyles.optionButton} ${
                selectedOption === option ? modalStyles.optionSelected : ""
              }`}
              onClick={() => setSelectedOption(option)}
            >
              <span className={modalStyles.optionText}>{option}</span>
            </button>
          ))}
        </div>
      );
    }
  };

  if (isGameOver && gameDetails) {
    return (
      <div className={styles.resultsCard}>
        <h2>Koniec gry! 🏁</h2>
        <div className={styles.statsGrid}>
          <p>
            Wszystkie pytania: <strong>{gameDetails.questionsCount}</strong>
          </p>
          <p className={styles.correct}>
            Poprawne: <strong>{gameDetails.correctAnswers}</strong>
          </p>
          <p className={styles.wrong}>
            Błędne: <strong>{gameDetails.wrongAnswers}</strong>
          </p>
        </div>
        <button className={styles.restartButton} onClick={startGame}>
          Zagraj ponownie
        </button>
      </div>
    );
  }

  return (
    <div className={styles.gameContainer}>
      {!sessionId ? (
        <div className={styles.startSection}>
          <h1>Tryb Piaskownicy 🏖️</h1>
          <p>Odpowiadaj na pytania, dopóki starczy Ci czasu!</p>
          <p>Masz minutę!</p>
          <button
            className={styles.startButton}
            onClick={startGame}
            disabled={isLoading}
          >
            {isLoading ? "Ładowanie..." : "Rozpocznij"}
          </button>
        </div>
      ) : (
        <div className={styles.activeGame}>
          <div
            className={`${styles.timer} ${
              timeLeft <= 10 ? styles.timerUrgent : ""
            }`}
          >
            ⏱️ {formatTime(timeLeft)}
          </div>

          {currentExercise ? (
            <div className={styles.questionCard}>
              <h3 className={styles.pathTitle}>
                {currentExercise.learningPath}
              </h3>
              <p className={styles.content}>{currentExercise.content}</p>

              <div className={styles.answerArea}>{renderAnswerSection()}</div>

              <button
                className={styles.submitButton}
                onClick={handleSubmitAnswer}
                disabled={!userAnswer && !userNumericAnswer && !selectedOption}
              >
                Wyślij odpowiedź
              </button>
            </div>
          ) : (
            <div className={styles.loadingQuestion}>
              Przygotowywanie pytania...
            </div>
          )}
        </div>
      )}
    </div>
  );
}
