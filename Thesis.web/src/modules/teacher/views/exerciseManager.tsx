import { useState, useEffect } from "react";
import StudentService, {
  type PathExercisesResource,
} from "../../student/studentService";
import ExerciseService, {
  type ExerciseDetails,
} from "../../exercise/ExerciseService";
import styles from "../styles/teacherDashboard.module.css";
import studentStyles from "../../student/styles/StudentComponents.module.css";
import ExerciseModal from "../../exercise/modals/ExerciseModal";
import { toast } from "react-toastify";

const ExerciseManager = ({ path, onBack, onPublish, onRemove }: any) => {
  const [isAdding, setIsAdding] = useState(false);
  const [exercisesData, setExercisesData] =
    useState<PathExercisesResource | null>(null);
  const [selectedExercise, setSelectedExercise] =
    useState<ExerciseDetails | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [exType, setExType] = useState<"TEXT" | "ABCD">("TEXT");
  const [content, setContent] = useState("");
  const [answer, setAnswer] = useState({
    correctOption: "",
    incorrectOption1: "",
    incorrectOption2: "",
    incorrectOption3: "",
    correctText: "",
  });

  const fetchPathExercises = async () => {
    try {
      const data = await StudentService.GetPathExercise(path.publicId);
      setExercisesData(data);
    } catch (error) {
      console.error("Błąd pobierania zadań:", error);
    }
  };

  useEffect(() => {
    fetchPathExercises();
  }, [path.publicId]);

  const handleExerciseClick = async (exerciseId: string) => {
    try {
      const data = await ExerciseService.GetExercise(exerciseId);
      setSelectedExercise(data);
      setIsModalOpen(true);
    } catch (error) {
      console.error("Błąd pobierania szczegółów zadania:", error);
    }
  };

  const handleSaveExercise = async () => {
    let finalAnswer: any = {};

    if (exType === "TEXT") {
      const trimmedValue = answer.correctText.trim();

      const numericValue = Number(trimmedValue.replace(",", "."));
      const isNumber = trimmedValue !== "" && !isNaN(numericValue);

      finalAnswer = {
        correctText: isNumber ? null : trimmedValue,
        correctNumber: isNumber ? numericValue : null,
        correctOption: null,
        incorrectOption1: null,
        incorrectOption2: null,
        incorrectOption3: null,
      };
    } else {
      finalAnswer = {
        correctOption: answer.correctOption,
        incorrectOption1: answer.incorrectOption1,
        incorrectOption2: answer.incorrectOption2,
        incorrectOption3: answer.incorrectOption3,
        correctText: null,
        correctNumber: null,
      };
    }

    const model = {
      content,
      answer: finalAnswer,
    };

    try {
      await StudentService.CreatePathExercise(path.publicId, model);
      toast.success("Dodano zadanie do szkicu!", {
        position: "bottom-center",
        autoClose: 3000,
        className: "custom-toast",
      });

      setIsAdding(false);
      setContent("");
      setAnswer({
        correctOption: "",
        incorrectOption1: "",
        incorrectOption2: "",
        incorrectOption3: "",
        correctText: "",
      });
      fetchPathExercises();
    } catch (error) {
      toast.error("Wystąpił błąd podczas zapisywania.");
    }
  };

  return (
    <div className={styles.contentSection}>
      <button onClick={onBack} className={studentStyles.backButton}>
        ← Powrót do listy szkiców
      </button>

      <div className={studentStyles.pathHeader} style={{ marginTop: "24px" }}>
        <div className={studentStyles.pathHeaderContent}>
          <span className={studentStyles.pathEmoji}>⚙️</span>
          <div>
            <h2 className={studentStyles.pathTitle}>
              Zarządzanie: {path.name}
            </h2>
            <p className={studentStyles.pathMeta}>
              Poziom {path.level} • {exercisesData?.count || 0} zadań w kolejce
            </p>
          </div>
        </div>

        <div className={styles.classCreatorActions} style={{ marginTop: 0 }}>
          <button
            onClick={onPublish}
            className={styles.selectClassButton}
            style={{
              background: "linear-gradient(135deg, #28a745 0%, #34ce57 100%)",
              boxShadow: "0 4px 12px rgba(40, 167, 69, 0.35)",
            }}
          >
            Opublikuj Ścieżkę
          </button>
          <button onClick={onRemove} className={styles.cancelButton}>
            Usuń Szkic
          </button>
        </div>
      </div>

      <div style={{ margin: "40px 0" }}>
        <h3 className={styles.sidebarTitle} style={{ marginBottom: "16px" }}>
          Struktura ścieżki
        </h3>
        <div className={studentStyles.exercisesGrid}>
          {exercisesData?.exercises.map((exercise, index) => (
            <div
              key={exercise.publicId}
              className={`${studentStyles.exerciseBox} ${studentStyles.exerciseIncomplete}`}
              onClick={() => handleExerciseClick(exercise.publicId)}
            >
              <span className={studentStyles.exerciseNumber}>{index + 1}</span>
            </div>
          ))}
          <div
            className={studentStyles.exerciseBox}
            style={{
              border: "2px dashed #5b4fc4",
              background: "rgba(91, 79, 196, 0.05)",
              cursor: "pointer",
            }}
            onClick={() => setIsAdding(true)}
          >
            <span
              className={studentStyles.exerciseNumber}
              style={{ color: "#5b4fc4" }}
            >
              +
            </span>
          </div>
        </div>
      </div>

      {isAdding && (
        <div
          className={styles.contentSection}
          style={{
            marginTop: "30px",
            border: "2px solid #5b4fc4",
            padding: "32px",
          }}
        >
          <div
            className={styles.sidebarHeader}
            style={{ padding: 0, marginBottom: "24px", border: "none" }}
          >
            <h3 className={styles.sidebarTitle}>🆕 Tworzenie nowego zadania</h3>
          </div>

          <div className={styles.classCreatorForm}>
            <div className={styles.navSection}>
              <label className={styles.navSectionTitle}>
                Treść pytania lub polecenia
              </label>
              <textarea
                className={styles.classCreatorInput}
                style={{
                  minHeight: "160px",
                  maxHeight: "350px",
                  resize: "vertical",
                  fontSize: "1.1rem",
                  lineHeight: "1.5",
                }}
                placeholder="Wpisz tutaj treść zadania, którą zobaczy uczeń..."
                value={content}
                onChange={(e) => setContent(e.target.value)}
              />
            </div>

            <div className={styles.navSection}>
              <label className={styles.navSectionTitle}>
                Format odpowiedzi
              </label>
              <div
                className={styles.classCreatorActions}
                style={{ gap: "12px" }}
              >
                <button
                  type="button"
                  className={
                    exType === "TEXT" ? styles.navItemActive : styles.navItem
                  }
                  style={{
                    flex: 1,
                    justifyContent: "center",
                    borderRadius: "12px",
                    border: "1px solid #e5e7eb",
                  }}
                  onClick={() => setExType("TEXT")}
                >
                  <span className={styles.navIcon}>⌨️</span>
                  <span className={styles.navText}>Tekst / Liczba</span>
                </button>
                <button
                  type="button"
                  className={
                    exType === "ABCD" ? styles.navItemActive : styles.navItem
                  }
                  style={{
                    flex: 1,
                    justifyContent: "center",
                    borderRadius: "12px",
                    border: "1px solid #e5e7eb",
                  }}
                  onClick={() => setExType("ABCD")}
                >
                  <span className={styles.navIcon}>📑</span>
                  <span className={styles.navText}>Wybór ABCD</span>
                </button>
              </div>
            </div>

            <div
              className={studentStyles.progressSection}
              style={{ padding: "24px", background: "#fcfcff" }}
            >
              <label
                className={styles.navSectionTitle}
                style={{ paddingLeft: 0, marginBottom: "12px" }}
              >
                Parametry poprawnej odpowiedzi
              </label>

              {exType === "TEXT" ? (
                <div>
                  <input
                    className={styles.classCreatorInput}
                    style={{ width: "100%", border: "2px solid #28a745" }}
                    placeholder="Wpisz poprawną frazę lub liczbę..."
                    value={answer.correctText}
                    onChange={(e) =>
                      setAnswer({ ...answer, correctText: e.target.value })
                    }
                  />
                  <p
                    className={styles.classPrompt}
                    style={{ marginTop: "10px" }}
                  >
                    💡 System automatycznie rozpozna, czy wpisana wartość to
                    liczba czy tekst.
                  </p>
                </div>
              ) : (
                <div
                  style={{
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr",
                    gap: "16px",
                  }}
                >
                  <div className={styles.navSection}>
                    <label
                      className={styles.classLabel}
                      style={{ color: "#28a745" }}
                    >
                      Poprawna (A)
                    </label>
                    <input
                      className={styles.classCreatorInput}
                      style={{ borderColor: "#28a745" }}
                      placeholder="Prawidłowa opcja"
                      value={answer.correctOption}
                      onChange={(e) =>
                        setAnswer({ ...answer, correctOption: e.target.value })
                      }
                    />
                  </div>
                  <div className={styles.navSection}>
                    <label className={styles.classLabel}>Błędna (B)</label>
                    <input
                      className={styles.classCreatorInput}
                      placeholder="Fałszywa opcja"
                      value={answer.incorrectOption1}
                      onChange={(e) =>
                        setAnswer({
                          ...answer,
                          incorrectOption1: e.target.value,
                        })
                      }
                    />
                  </div>
                  <div className={styles.navSection}>
                    <label className={styles.classLabel}>Błędna (C)</label>
                    <input
                      className={styles.classCreatorInput}
                      placeholder="Fałszywa opcja"
                      value={answer.incorrectOption2}
                      onChange={(e) =>
                        setAnswer({
                          ...answer,
                          incorrectOption2: e.target.value,
                        })
                      }
                    />
                  </div>
                  <div className={styles.navSection}>
                    <label className={styles.classLabel}>Błędna (D)</label>
                    <input
                      className={styles.classCreatorInput}
                      placeholder="Fałszywa opcja"
                      value={answer.incorrectOption3}
                      onChange={(e) =>
                        setAnswer({
                          ...answer,
                          incorrectOption3: e.target.value,
                        })
                      }
                    />
                  </div>
                </div>
              )}
            </div>

            <div
              className={styles.classCreatorActions}
              style={{ justifyContent: "flex-end", marginTop: "24px" }}
            >
              <button
                onClick={() => setIsAdding(false)}
                className={styles.cancelButton}
              >
                Anuluj
              </button>
              <button
                onClick={handleSaveExercise}
                className={styles.selectClassButton}
                disabled={
                  !content.trim() ||
                  (exType === "TEXT"
                    ? !answer.correctText.trim()
                    : !answer.correctOption.trim())
                }
              >
                Dodaj zadanie do ścieżki
              </button>
            </div>
          </div>
        </div>
      )}

      <ExerciseModal
        exercise={selectedExercise}
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        isDone={true}
        isTeacherView={true}
      />
    </div>
  );
};

export default ExerciseManager;
