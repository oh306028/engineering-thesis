import React, { useEffect, useState } from "react";
import styles from "../styles/StudentComponents.module.css";
import ClassroomService, {
  type ClassroomDetails,
  type HomeworkDetails,
} from "../../classroom/ClassroomService.tsx";

function Homework() {
  const [classroom, setClassroom] = useState<ClassroomDetails | null>(null);
  const [homework, setHomework] = useState<HomeworkDetails[]>([]);
  const [loading, setLoading] = useState(true);
  const [expandedHomework, setExpandedHomework] = useState<string | null>(null);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const classroomData = await ClassroomService.GetMineClassroom();
      setClassroom(classroomData);

      if (classroomData) {
        const homeworkData = await ClassroomService.GetMyHomeWork();
        setHomework(homeworkData);
      }
    } catch (error) {
      console.error("Błąd podczas pobierania zadań domowych:", error);
    } finally {
      setLoading(false);
    }
  };

  const formatDeadline = (deadline: string) => {
    const date = new Date(deadline);
    const now = new Date();
    const diffTime = date.getTime() - now.getTime();
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    if (diffDays < 0) {
      return "Termin minął";
    } else if (diffDays === 0) {
      return "Dziś";
    } else if (diffDays === 1) {
      return "Jutro";
    } else {
      return `Za ${diffDays} dni`;
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("pl-PL", {
      day: "numeric",
      month: "long",
      year: "numeric",
    });
  };

  const isOverdue = (deadline: string) => {
    const date = new Date(deadline);
    const now = new Date();
    return date.getTime() < now.getTime();
  };

  const toggleExpanded = (homeworkId: string) => {
    if (expandedHomework === homeworkId) {
      setExpandedHomework(null);
    } else {
      setExpandedHomework(homeworkId);
    }
  };

  if (loading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie zadań domowych...</p>
      </div>
    );
  }

  if (!classroom) {
    return (
      <div className={styles.contentSection}>
        <div className={styles.header}>
          <h2 className={styles.title}>
            <span className={styles.icon}>📝</span>
            Zadania Domowe
          </h2>
          <p className={styles.subtitle}>
            Sprawdź swoje zadania i terminy oddania
          </p>
        </div>

        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>🔒</span>
          <p>Musisz dołączyć do klasy</p>
          <p className={styles.subtitle}>
            Aby zobaczyć zadania domowe, najpierw dołącz do klasy w zakładce
            "Ranking uczniów"
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>📝</span>
          Zadania Domowe
        </h2>
        <p className={styles.subtitle}>
          Klasa: {classroom.className} • Nauczyciel: {classroom.teacherName}
        </p>
      </div>

      {/* Stats Card */}
      <div className={styles.pointsCard}>
        <div className={styles.pointsContent}>
          <span className={styles.pointsIcon}>📋</span>
          <div>
            <div className={styles.pointsLabel}>Zadań do wykonania</div>
            <div className={styles.pointsValue}>{homework.length}</div>
          </div>
        </div>
        <div className={styles.levelBadge}>
          <span className={styles.levelIcon}>📚</span>
          <span>Do szkoły</span>
        </div>
      </div>

      {homework.length === 0 ? (
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>📚</span>
          <p>Brak zadań domowych</p>
          <p className={styles.subtitle}>
            Nauczyciel jeszcze nie dodał żadnych zadań
          </p>
        </div>
      ) : (
        <div className={styles.achievementsSection}>
          <h3 className={styles.sectionTitle}>Lista zadań domowych</h3>
          <div className={styles.homeworkList}>
            {homework.map((hw) => (
              <div
                key={hw.publicId}
                className={`${styles.homeworkCard} ${
                  isOverdue(hw.deadLine) ? styles.urgent : ""
                }`}
              >
                <div className={styles.homeworkHeader}>
                  <div>
                    <h4 className={styles.homeworkTitle}>{hw.title}</h4>
                    <p className={styles.homeworkSubject}>
                      {hw.subject} • {hw.type}
                    </p>
                  </div>
                  <div
                    className={
                      isOverdue(hw.deadLine)
                        ? styles.deadline
                        : styles.deadlineCompleted
                    }
                  >
                    {formatDeadline(hw.deadLine)}
                  </div>
                </div>

                <p className={styles.homeworkDescription}>{hw.description}</p>

                {hw.exercises && hw.exercises.length > 0 && (
                  <div style={{ marginTop: "20px" }}>
                    <div
                      style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        marginBottom: "12px",
                      }}
                    >
                      <span
                        style={{
                          fontSize: "0.9rem",
                          fontWeight: 700,
                          color: "#5b4fc4",
                        }}
                      >
                        📝 Zadania do wykonania ({hw.exercises.length})
                      </span>
                      <button
                        onClick={() => toggleExpanded(hw.publicId)}
                        className={styles.secondaryButton}
                        style={{ padding: "6px 12px", fontSize: "0.85rem" }}
                      >
                        {expandedHomework === hw.publicId
                          ? "Zwiń ▲"
                          : "Rozwiń ▼"}
                      </button>
                    </div>

                    {expandedHomework === hw.publicId && (
                      <ul className={styles.rewardsList}>
                        {hw.exercises.map((exercise, index) => (
                          <li key={index}>{exercise.content}</li>
                        ))}
                      </ul>
                    )}
                  </div>
                )}

                <div className={styles.homeworkFooter}>
                  <span className={styles.status}>
                    {isOverdue(hw.deadLine)
                      ? "⚠️ Po terminie"
                      : "📌 Przynieś do szkoły"}
                  </span>
                  <span
                    style={{
                      fontSize: "0.85rem",
                      color: "#6b7280",
                      fontWeight: 600,
                    }}
                  >
                    Zadano: {formatDate(hw.dateCreated)}
                  </span>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

export default Homework;
