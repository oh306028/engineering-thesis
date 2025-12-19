import React, { useEffect, useState } from "react";
import styles from "../styles/teacherDashboard.module.css";
import studentStyles from "../../student/styles/StudentComponents.module.css";
import StudentService, {
  type LearningPathDetails,
} from "../../student/studentService";
import ExerciseManager from "./exerciseManager";
import PathForm from "./pathForm";

type CreatorStep = "LIST" | "CREATE_PATH" | "MANAGE_EXERCISES";

const levelEmojis: { [key: number]: string } = {
  1: "🌱",
  2: "🌿",
  3: "🌳",
  4: "🏔️",
  5: "⭐",
};

export default function ContentCreator() {
  const [step, setStep] = useState<CreatorStep>("LIST");
  const [drafts, setDrafts] = useState<LearningPathDetails[]>([]);
  const [selectedPath, setSelectedPath] = useState<LearningPathDetails | null>(
    null
  );
  const [isLoading, setIsLoading] = useState(false);

  const fetchDrafts = async () => {
    setIsLoading(true);
    try {
      const data = await StudentService.GetDrafts();
      setDrafts(data);
    } catch (e) {
      console.error(e);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchDrafts();
  }, []);

  const handlePublish = async (id: string) => {
    if (!window.confirm("Czy na pewno chcesz opublikować tę ścieżkę?")) return;
    await StudentService.Publish(id);
    fetchDrafts();
    setStep("LIST");
  };

  const handleRemove = async (id: string) => {
    if (!window.confirm("Usunąć szkic?")) return;
    await StudentService.RemoveDraft(id);
    fetchDrafts();
  };

  if (step === "CREATE_PATH") {
    return (
      <PathForm
        onCancel={() => setStep("LIST")}
        onSuccess={() => {
          setStep("LIST");
          fetchDrafts();
        }}
      />
    );
  }

  if (step === "MANAGE_EXERCISES" && selectedPath) {
    return (
      <ExerciseManager
        path={selectedPath}
        onBack={() => setStep("LIST")}
        onPublish={() => handlePublish(selectedPath.publicId)}
        onRemove={() => {
          handleRemove(selectedPath.publicId);
          setStep("LIST");
        }}
      />
    );
  }

  return (
    <div className={studentStyles.contentSection}>
      <div className={studentStyles.header}>
        <h2 className={studentStyles.title}>
          <span className={studentStyles.icon}>🛠️</span>
          Twoje wersje robocze Ścieżek
        </h2>
        <button
          className={styles.selectClassButton}
          onClick={() => setStep("CREATE_PATH")}
          style={{ marginTop: "0px" }}
        >
          + Nowa wersja robocza
        </button>
      </div>

      {drafts.length === 0 ? (
        <div className={studentStyles.emptyState}>
          <span className={studentStyles.emptyIcon}>📚</span>
          <p>Brak wersji roboczych. Stwórz nową ścieżkę!</p>
        </div>
      ) : (
        <div className={studentStyles.pathsGrid}>
          {drafts.map((path) => (
            <div
              key={path.publicId}
              className={studentStyles.pathCard}
              onClick={() => {
                setSelectedPath(path);
                setStep("MANAGE_EXERCISES");
              }}
            >
              <div className={studentStyles.pathCardHeader}>
                <span className={studentStyles.pathCardEmoji}>
                  {levelEmojis[path.level] || "🎯"}
                </span>
                <div className={studentStyles.levelBadgeSmall}>
                  Poziom {path.level}
                </div>
              </div>

              <h3 className={studentStyles.pathCardTitle}>{path.name}</h3>
              <p className={studentStyles.pathCardType}>
                {path.subject || "Matematyka"}
              </p>

              <button className={studentStyles.pathCardButton}>
                Edytuj szkic
                <span className={studentStyles.arrowIcon}>→</span>
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
