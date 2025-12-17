import React, { useEffect, useState, useCallback } from "react";
import styles from "../styles/joinRequests.module.css";
import ClassroomService, {
  type StudentDetails,
} from "../../classroom/ClassroomService.tsx";

interface JoinRequestsViewProps {
  classId: string;
}

const JoinRequestsView: React.FC<JoinRequestsViewProps> = ({ classId }) => {
  const [requests, setRequests] = useState<StudentDetails[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchRequests = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await ClassroomService.GetClassroomRequests(classId);
      setRequests(data);
    } catch (err) {
      console.error("Błąd ładowania próśb:", err);
      setError(
        "Nie udało się załadować próśb o dołączenie. Spróbuj ponownie później."
      );
    } finally {
      setIsLoading(false);
    }
  }, [classId]);

  useEffect(() => {
    fetchRequests();
  }, [fetchRequests]);

  const handleAction = async (
    studentPublicId: string,
    action: "accept" | "decline"
  ) => {
    setRequests(
      (prevRequests) =>
        prevRequests.map((req) =>
          req.publicId === studentPublicId
            ? { ...req, isProcessing: true }
            : req
        ) as (StudentDetails & { isProcessing?: boolean })[]
    );

    try {
      if (action === "accept") {
        await ClassroomService.AcceptStudent(classId, studentPublicId);
      } else {
        await ClassroomService.DeclineStudent(classId, studentPublicId);
      }

      setRequests(
        (prevRequests) =>
          prevRequests.filter(
            (req) => req.publicId !== studentPublicId
          ) as StudentDetails[]
      );
    } catch (err) {
      console.error(
        `Błąd podczas ${action === "accept" ? "akceptacji" : "odrzucania"}:`,
        err
      );
      setError(
        `Nie udało się ${
          action === "accept" ? "zaakceptować" : "odrzucić"
        } prośby.`
      );

      setRequests(
        (prevRequests) =>
          prevRequests.map((req) =>
            req.publicId === studentPublicId
              ? { ...req, isProcessing: false }
              : req
          ) as (StudentDetails & { isProcessing?: boolean })[]
      );
    }
  };

  if (isLoading) {
    return (
      <div className={styles.contentSection}>
        <p className={styles.loadingState}>Ładowanie próśb o dołączenie...</p>
      </div>
    );
  }

  return (
    <div className={styles.contentSection}>
      <h2 className={styles.contentTitle}>🚪 Prośby o dołączenie do Klasy</h2>
      <p className={styles.contentDescription}>
        Zarządzaj prośbami uczniów o dołączenie do Twojej klasy.
      </p>

      {error && <div className={styles.errorMessage}>{error}</div>}

      {requests.length === 0 ? (
        <div className={styles.emptyRequests}>
          <span className={styles.emptyIcon}>🎉</span>
          <p>Brak nowych próśb o dołączenie do tej klasy.</p>
        </div>
      ) : (
        <ul className={styles.requestList}>
          {requests.map((student) => {
            const isProcessing = (student as any).isProcessing;
            return (
              <li key={student.publicId} className={styles.requestItem}>
                <div className={styles.studentInfo}>
                  <span className={styles.studentName}>{student.name}</span>
                  <span className={styles.studentDetails}>
                    Poziom: **{student.level}** | EXP: **{student.currentPoints}
                    **
                  </span>
                </div>
                <div className={styles.actionButtons}>
                  <button
                    className={`${styles.acceptButton} ${
                      isProcessing ? styles.processingButton : ""
                    }`}
                    onClick={() => handleAction(student.publicId, "accept")}
                    disabled={isProcessing}
                  >
                    <span className={styles.navIcon}>✔️</span>
                  </button>
                  <button
                    className={`${styles.declineButton} ${
                      isProcessing ? styles.processingButton : ""
                    }`}
                    onClick={() => handleAction(student.publicId, "decline")}
                    disabled={isProcessing}
                  >
                    <span className={styles.navIcon}>✖️</span>
                  </button>
                </div>
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
};

export default JoinRequestsView;
