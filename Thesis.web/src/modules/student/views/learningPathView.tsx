import React, { useEffect, useState } from "react";
import styles from "../styles/StudentComponents.module.css";
import StudentService, {
  type LearningPathDetails,
  type PathExercisesResource,
} from "../studentService";
import ExerciseModal from "../../exercise/modals/ExerciseModal";
import type { ExerciseDetails } from "../../exercise/ExerciseService";
import ExerciseService from "../../exercise/ExerciseService";

const levelEmojis: { [key: number]: string } = {
  0: "🌱",
  1: "🌿",
  2: "🌳",
  3: "🏔️",
  4: "⭐",
};

export interface LearningViewProps {
  onClose: () => void;
}

interface LearningPathViewProps {
  pathType: "Regular" | "Challenge" | "Review";
  title: string;
  subtitle: string;
  icon: string;
  onClose: () => void;
}

function LearningPathView({
  pathType,
  title,
  subtitle,
  icon,
  onClose,
}: LearningPathViewProps) {
  const [paths, setPaths] = useState<LearningPathDetails[]>([]);
  const [selectedPath, setSelectedPath] = useState<LearningPathDetails | null>(
    null
  );
  const [exercises, setExercises] = useState<PathExercisesResource | null>(
    null
  );
  const [isLoading, setIsLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedExercise, setSelectedExercise] =
    useState<ExerciseDetails | null>(null);

  const [isDone, setIsDone] = useState<boolean>(false);

  useEffect(() => {
    const fetchPaths = async () => {
      try {
        setIsLoading(true);
        const data = await StudentService.GetPaths(pathType);
        const selectedPaths = data.filter(
          (p) => p.isCurrentPathFinished === false
        );
        setPaths(selectedPaths);
      } catch (error) {
        console.error("Błąd pobierania ścieżek:", error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchPaths();
  }, [pathType]);

  const handlePathClick = async (path: LearningPathDetails) => {
    setSelectedPath(path);
    try {
      fetchPathExercises(path.publicId);
    } catch (error) {
      console.error("Błąd pobierania zadań:", error);
    }
  };

  const fetchPathExercises = async (pathId: string) => {
    if (pathType === "Review") {
      const data = await StudentService.GetPathExercise(pathId, true);
      setExercises(data);
    } else {
      const data = await StudentService.GetPathExercise(pathId);
      setExercises(data);
    }
  };

  const handleBackToList = () => {
    setSelectedPath(null);
    setExercises(null);
  };

  const handleExerciseClick = async (
    exerciseId: string,
    isCompleted: boolean
  ) => {
    try {
      const data = await ExerciseService.GetExercise(exerciseId);
      setSelectedExercise(data);
      setIsModalOpen(true);
      setIsDone(isCompleted);
    } catch (error) {
      console.error("Błąd pobierania szczegółów zadania:", error);
    }
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedExercise(null);
    fetchPathExercises(selectedPath!.publicId);
    onClose();
  };

  if (isLoading) {
    return (
      <div className={styles.contentSection}>
        <div className={styles.loadingContainer}>
          <div className={styles.spinner}></div>
          <p>Ładowanie ścieżek nauki...</p>
        </div>
      </div>
    );
  }

  if (selectedPath && exercises) {
    return (
      <div className={styles.contentSection}>
        <button onClick={handleBackToList} className={styles.backButton}>
          ← Powrót do listy ścieżek
        </button>

        <div className={styles.pathHeader}>
          <div className={styles.pathHeaderContent}>
            <span className={styles.pathEmoji}>
              {levelEmojis[selectedPath.level] || "🎯"}
            </span>
            <div>
              <h2 className={styles.pathTitle}>{selectedPath.name}</h2>
              {pathType === "Review" && (
                <p className={styles.pathMeta}>
                  Ścieżka, w której przygotowujemy zadania, z którymi masz
                  największy problem!
                </p>
              )}
              {pathType !== "Review" && (
                <p className={styles.pathMeta}>
                  Poziom {selectedPath.level} • {selectedPath.type}
                </p>
              )}
            </div>
          </div>

          <div>
            <div className={styles.pathProgress}>
              <span className={styles.completedCount}>
                {exercises.exercises.filter((e) => e.isCompleted).length}
              </span>
              <span className={styles.totalCount}>/ {exercises.count}</span>
            </div>

            <div className={styles.badgesGrid2}>
              {selectedPath.badges.map((badge) => (
                <div key={badge.name} className={styles.badge2}>
                  <span className={styles.badgeIcon2}>{badge.emote}</span>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className={styles.exercisesGrid}>
          {exercises.exercises.map((exercise, index) => (
            <div
              key={exercise.publicId}
              className={`${styles.exerciseBox} ${
                exercise.isCompleted
                  ? styles.exerciseCompleted
                  : styles.exerciseIncomplete
              }`}
              onClick={() =>
                handleExerciseClick(exercise.publicId, exercise.isCompleted)
              }
            >
              <span className={styles.exerciseNumber}>{index + 1}</span>
              {exercise.isCompleted && (
                <span className={styles.checkmark}>✓</span>
              )}
            </div>
          ))}
        </div>

        <ExerciseModal
          exercise={selectedExercise}
          isOpen={isModalOpen}
          onClose={handleCloseModal}
          isDone={isDone}
          isTeacherView={false}
        />
      </div>
    );
  }

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>{icon}</span>
          {title}
        </h2>
        <p className={styles.subtitle}>{subtitle}</p>
      </div>

      {paths.length === 0 ? (
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>📚</span>
          <p>Brak dostępnych ścieżek nauki</p>
        </div>
      ) : (
        <div className={styles.pathsGrid}>
          {paths.map((path) => (
            <div
              key={path.publicId}
              className={styles.pathCard}
              onClick={() => handlePathClick(path)}
            >
              <div className={styles.pathCardHeader}>
                <span className={styles.pathCardEmoji}>
                  {levelEmojis[path.level] || "🎯"}
                </span>
                <div className={styles.levelBadgeSmall}>
                  Poziom {path.level}
                </div>
              </div>
              <h3 className={styles.pathCardTitle}>{path.name}</h3>
              <p className={styles.pathCardType}>{path.subject}</p>
              <button className={styles.pathCardButton}>
                Rozpocznij
                <span className={styles.arrowIcon}>→</span>
              </button>
            </div>
          ))}
        </div>
      )}

      <ExerciseModal
        exercise={selectedExercise}
        isOpen={isModalOpen}
        onClose={handleCloseModal}
        isDone={isDone}
        isTeacherView={false}
      />
    </div>
  );
}

export default LearningPathView;
