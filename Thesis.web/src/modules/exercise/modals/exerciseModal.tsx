import React, { useState, useEffect } from "react";
import styles from "../styles/exerciseModal.module.css";
import type { AnswerModel, ExerciseDetails } from "../ExerciseService";
import ExerciseService from "../ExerciseService";
import { toast } from "react-toastify";

interface ExerciseModalProps {
  exercise: ExerciseDetails | null;
  isOpen: boolean;
  isDone: boolean;
  onClose: () => void;
}

function ExerciseModal({
  exercise,
  isOpen,
  onClose,
  isDone,
}: ExerciseModalProps) {
  const [userAnswer, setUserAnswer] = useState<string>("");
  const [userNumericAnswer, setUserNumericAnswer] = useState<string>("");
  const [selectedOption, setSelectedOption] = useState<string>("");
  const [shuffledOptions, setShuffledOptions] = useState<string[]>([]);
  const [answerFailed, setAnswerFailed] = useState<string>("");

  useEffect(() => {
    if (!isOpen) {
      setUserAnswer("");
      setSelectedOption("");
      setUserNumericAnswer("");
      setAnswerFailed("");
    }
  }, [isOpen]);

  // Tasowanie opcji raz dla każdego zadania
  useEffect(() => {
    if (!exercise || !exercise.answer) return;

    const { answer } = exercise;

    if (answer.correctOption && answer.correctOption.trim() !== "") {
      const options = [
        answer.correctOption,
        answer.incorrectOption1,
        answer.incorrectOption2,
        answer.incorrectOption3,
      ].filter((opt) => opt && opt.trim() !== "");

      setShuffledOptions([...options].sort(() => Math.random() - 0.5));
    } else {
      setShuffledOptions([]);
    }

    // reset pól przy nowym zadaniu
    setSelectedOption("");
    setUserAnswer("");
    setUserNumericAnswer("");
    setAnswerFailed("");
  }, [exercise]);

  if (!isOpen || !exercise) return null;

  const handleBackdropClick = (e: React.MouseEvent) => {
    if (e.target === e.currentTarget) {
      onClose();
    }
  };

  const handleSubmit = async () => {
    const numericValue =
      userNumericAnswer !== "" ? Number(userNumericAnswer) : null;

    const answer: AnswerModel = {
      correctText: userAnswer,
      correctOption: selectedOption,
      correctNumber: numericValue,
    };

    try {
      await ExerciseService.Answer(exercise.publicId, answer);

      toast.success("Zadanie wykonane poprawnie!", {
        position: "bottom-center",
        autoClose: 10000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        className: "custom-toast",
        onClose: () => {
          onClose();
        },
      });
    } catch (error: any) {
      if (error.response?.status === 422) {
        setAnswerFailed(error.response.data.error);
      }
    }
  };

  const renderAnswerField = () => {
    if (!exercise.answer) {
      return (
        <div className={styles.answerSection}>
          <p className={styles.errorText}>
            Brak danych odpowiedzi dla tego zadania.
          </p>
        </div>
      );
    } else if (isDone) {
      return (
        <div className={styles.answerSection}>
          <p className={styles.successText}>
            Zadanie zostało poprawnie wykonane!
          </p>
        </div>
      );
    }

    const { answer } = exercise;

    // Tekst
    if (answer.correctText && answer.correctText.trim() !== "") {
      return (
        <div className={styles.answerSection}>
          <label className={styles.label}>Twoja odpowiedź:</label>
          <textarea
            className={styles.textarea}
            value={userAnswer}
            onChange={(e) => {
              setUserAnswer(e.target.value);
              setAnswerFailed("");
            }}
            placeholder="Wpisz swoją odpowiedź..."
            rows={4}
          />
        </div>
      );
    }

    // Liczba
    if (answer.correctNumber !== undefined && answer.correctNumber !== null) {
      return (
        <div className={styles.answerSection}>
          <label className={styles.label}>Twoja odpowiedź:</label>
          <input
            type="number"
            className={styles.numberInput}
            value={userNumericAnswer}
            onChange={(e) => {
              setUserNumericAnswer(e.target.value);
              setAnswerFailed("");
            }}
            placeholder="Wpisz liczbę..."
          />
        </div>
      );
    }

    // Opcje
    if (shuffledOptions.length > 0) {
      return (
        <div className={styles.answerSection}>
          <label className={styles.label}>Wybierz odpowiedź:</label>
          <div className={styles.optionsGrid}>
            {shuffledOptions.map((option, index) => (
              <button
                key={index}
                className={`${styles.optionButton} ${
                  selectedOption === option ? styles.optionSelected : ""
                }`}
                onClick={() => {
                  setSelectedOption(option);
                  setAnswerFailed("");
                }}
              >
                <span className={styles.optionLetter}>
                  {String.fromCharCode(65 + index)}
                </span>
                <span className={styles.optionText}>{option}</span>
              </button>
            ))}
          </div>
        </div>
      );
    }

    return (
      <div className={styles.answerSection}>
        <p className={styles.errorText}>Nie można określić typu zadania.</p>
      </div>
    );
  };

  const isAnswerProvided = () => {
    if (!exercise.answer) return false;

    if (
      exercise.answer.correctOption &&
      exercise.answer.correctOption.trim() !== ""
    ) {
      return selectedOption !== "";
    }
    if (exercise.answer.correctText) return userAnswer.trim() !== "";
    if (exercise.answer.correctNumber) return userNumericAnswer !== "";
  };

  return (
    <div className={styles.backdrop} onClick={handleBackdropClick}>
      <div className={styles.modal}>
        <button className={styles.closeButton} onClick={onClose}>
          ✕
        </button>

        <div className={styles.modalHeader}>
          <div className={styles.headerTop}>
            <div className={styles.badges}>
              <span className={styles.subjectBadge}>{exercise.subject}</span>
              <span className={styles.levelBadge}>
                Poziom: {exercise.level}
              </span>
              {exercise.isDone && (
                <span className={styles.completedBadge}>✓ Ukończono</span>
              )}
            </div>
          </div>
          <h2 className={styles.modalTitle}>{exercise.learningPath}</h2>
        </div>

        <p className={styles.exerciseParagraph}>{exercise.content}</p>

        <div className={styles.modalContent}>{renderAnswerField()}</div>

        <p className={styles.inputError}>
          <span className={styles.errorMessage}>{answerFailed}</span>
        </p>
        <div className={styles.modalFooter}>
          <button className={styles.cancelButton} onClick={onClose}>
            Anuluj
          </button>
          {!isDone && (
            <button
              className={styles.submitButton}
              onClick={handleSubmit}
              disabled={!isAnswerProvided()}
            >
              Prześlij odpowiedź
            </button>
          )}
        </div>
      </div>
    </div>
  );
}

export default ExerciseModal;
