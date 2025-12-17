import React, { useEffect, useState } from "react";
import styles from "../styles/StudentComponents.module.css";
import ClassroomService, {
  type JoinClassroomModel,
  type ClassroomDetails,
  type StudentDetails,
} from "../../classroom/ClassroomService.tsx";
import dayjs from "dayjs";

interface MyClassProps {
  publicId: string | null;
}

const MyClass: React.FC<MyClassProps> = ({ publicId }) => {
  const [classroom, setClassroom] = useState<ClassroomDetails | null>(null);
  const [leaderboard, setLeaderboard] = useState<StudentDetails[]>([]);
  const [loading, setLoading] = useState(true);
  const [joining, setJoining] = useState(false);
  const [classCode, setClassCode] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    if (publicId === null) fetchClassroomData();
    else fetchClassroomDataForTeacher();
  }, []);

  const fetchClassroomData = async () => {
    try {
      const classroomData = await ClassroomService.GetMineClassroom();
      setClassroom(classroomData);

      const studentsData = await ClassroomService.GetStudentsForClassroom(
        classroomData!.publicId
      );

      setLeaderboard(studentsData);
    } catch (error) {
      console.error("Błąd podczas pobierania danych klasy:", error);
    } finally {
      setLoading(false);
    }
  };

  const fetchClassroomDataForTeacher = async () => {
    try {
      const studentsData = await ClassroomService.GetStudentsForClassroom(
        publicId!
      );

      setLeaderboard(studentsData);
    } catch (error) {
      console.error("Błąd podczas pobierania danych klasy:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleJoinClass = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    if (!classCode.trim()) {
      setError("Wprowadź kod klasy");
      return;
    }

    setJoining(true);
    try {
      await ClassroomService.JoinClassroom({
        classroomKey: classCode.trim(),
      } as JoinClassroomModel);
      await fetchClassroomData();
      setClassCode("");
    } catch (error: any) {
      console.log(error);
      setError(error.response.data.error);
    } finally {
      setJoining(false);
    }
  };

  if (loading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie danych klasy...</p>
      </div>
    );
  }

  if (!classroom && publicId === null) {
    return (
      <div className={styles.contentSection}>
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>🔒</span>
          <p>Nie należysz jeszcze do żadnej klasy</p>
          <p className={styles.subtitle}>
            Poproś swojego nauczyciela o kod klasy i dołącz poniżej
          </p>
           <p className={styles.subtitle}>
            Jeśli wysłałeś prośbę o dołączenie - poczekaj aż nauczyciel ją zaakceptuje.
          </p>
        </div>

        <div
          className={styles.card}
          style={{ maxWidth: "500px", margin: "32px auto 0" }}
        >
          <div className={styles.cardHeader}>
            <span className={styles.cardIcon}>🎓</span>
            <h3 className={styles.cardTitle}>Dołącz do klasy</h3>
          </div>

          <form onSubmit={handleJoinClass}>
            <div style={{ marginBottom: "20px" }}>
              <label
                htmlFor="classCode"
                style={{
                  display: "block",
                  marginBottom: "8px",
                  fontWeight: 600,
                  color: "#3b2f5c",
                }}
              >
                Kod klasy
              </label>
              <input
                id="classCode"
                type="text"
                value={classCode}
                onChange={(e) => setClassCode(e.target.value.toUpperCase())}
                placeholder="np. ABC123"
                disabled={joining}
                style={{
                  width: "100%",
                  padding: "14px",
                  fontSize: "1rem",
                  border: "2px solid #e5e7eb",
                  borderRadius: "12px",
                  fontWeight: 600,
                  letterSpacing: "0.1em",
                  textTransform: "uppercase",
                }}
              />
              {error && (
                <p
                  style={{
                    color: "#dc2626",
                    fontSize: "0.9rem",
                    marginTop: "8px",
                  }}
                >
                  {error}
                </p>
              )}
            </div>

            <button
              type="submit"
              className={styles.primaryButton}
              disabled={joining}
            >
              {joining ? "Dołączanie..." : "Dołącz do klasy"}
            </button>
          </form>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.contentSection}>
      {!publicId && (
        <>
          <div className={styles.contentSection}>
            <div className={styles.header}>
              <h2 className={styles.title}>
                <span className={styles.icon}>👥</span>
                {classroom?.className}
              </h2>
            </div>
          </div>

          <div className={styles.classInfo}>
            <div className={styles.infoCard}>
              <span className={styles.infoIcon}>🎓</span>
              <div>
                <div className={styles.infoLabel}>Uczniów w klasie</div>
                <div className={styles.infoValue}>{leaderboard.length}</div>
              </div>
            </div>

            <div className={styles.infoCard}>
              <span className={styles.infoIcon}>👨‍🏫</span>
              <div>
                <div className={styles.infoLabel}>Nauczyciel</div>
                <div className={styles.infoValue}>{classroom?.teacherName}</div>
              </div>
            </div>
          </div>
        </>
      )}

      <div className={styles.leaderboard}>
        <h3 className={styles.sectionTitle}>🏆 Ranking klasy</h3>

        {leaderboard.length === 0 ? (
          <div className={styles.emptyState}>
            <span className={styles.emptyIcon}>📊</span>
            <p>Brak danych rankingowych</p>
          </div>
        ) : (
          <div className={styles.leaderboardList}>
            {leaderboard.map((entry, index) => {
              const rank = index + 1;

              return (
                <div
                  key={index}
                  className={`${styles.leaderboardItem} ${
                    entry.isCurrentUser ? styles.currentUser : ""
                  }`}
                >
                  <div className={styles.rank}>
                    {rank === 1 && "🥇"}
                    {rank === 2 && "🥈"}
                    {rank === 3 && "🥉"}
                    {rank > 3 && `#${rank}`}
                  </div>
                  <div className={styles.studentName}>
                    {entry.name}
                    {entry.isCurrentUser && " (Ty)"}
                  </div>
                  <div className={styles.points}>
                    <span style={{ fontSize: "0.8rem", marginRight: "4px" }}>
                      {entry.currentPoints} pkt
                    </span>
                    Lvl: {entry.level} <br />
                    Odznaki: {entry.badgesCount}
                  </div>
                  <span>
                    Ostatnia aktywność:{" "}
                    {dayjs(entry.lastSeenAt).format("DD.MM.YYYY HH:mm")}
                  </span>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
};

export default MyClass;
