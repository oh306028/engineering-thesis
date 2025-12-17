import React, { useEffect, useState } from "react";
import styles from "../styles/AdminComponents.module.css";
import AdminService, {
  type TeacherAttemptsListModel,
} from "../adminService.tsx";

function TeacherRequests() {
  const [attempts, setAttempts] = useState<TeacherAttemptsListModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [processingId, setProcessingId] = useState<string | null>(null);

  useEffect(() => {
    fetchAttempts();
  }, []);

  const fetchAttempts = async () => {
    try {
      const data = await AdminService.GetTeacherAttempts();
      setAttempts(data);
    } catch (error) {
      console.error("Błąd podczas pobierania próśb:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleAccept = async (id: string) => {
    setProcessingId(id);
    try {
      await AdminService.AcceptTeacherAttempt(id);
      await fetchAttempts();
    } catch (error) {
      console.error("Błąd podczas akceptacji:", error);
    } finally {
      setProcessingId(null);
    }
  };

  const handleDecline = async (id: string) => {
    setProcessingId(id);
    try {
      await AdminService.DeclineTeacherAttempt(id);
      await fetchAttempts(); // Odśwież listę
    } catch (error) {
      console.error("Błąd podczas odrzucenia:", error);
    } finally {
      setProcessingId(null);
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

  if (loading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie próśb nauczycieli...</p>
      </div>
    );
  }

  const pendingAttempts = attempts.filter((a) => !a.isAccepted);

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>🙋‍♂️</span>
          Prośby Nauczycieli
        </h2>
        <p className={styles.subtitle}>
          Zarządzaj prośbami nauczycieli o dostęp do systemu
        </p>
      </div>

      <div className={styles.statsCard}>
        <div className={styles.statsContent}>
          <span className={styles.statsIcon}>📋</span>
          <div>
            <div className={styles.statsLabel}>Oczekujące prośby</div>
            <div className={styles.statsValue}>{pendingAttempts.length}</div>
          </div>
        </div>
      </div>

      {attempts.length === 0 ? (
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>📭</span>
          <p>Brak próśb nauczycieli</p>
        </div>
      ) : (
        <table className={styles.table}>
          <thead className={styles.tableHeader}>
            <tr>
              <th>Imię i nazwisko</th>
              <th>Login</th>
              <th>Email</th>
              <th>Data zgłoszenia</th>
              <th>Certyfikat</th>
              <th>Status</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            {attempts.map((attempt) => (
              <tr key={attempt.publicId} className={styles.tableRow}>
                <td className={styles.tableCell}>{attempt.fullName}</td>
                <td className={styles.tableCell}>{attempt.login}</td>
                <td className={styles.tableCell}>{attempt.email}</td>
                <td className={styles.tableCell}>
                  <span className={styles.dateText}>
                    {formatDate(attempt.dateCreated)}
                  </span>
                </td>
                <td className={styles.tableCell}>
                  <a
                    href={attempt.certificateUrl}
                    target="_blank"
                    rel="noopener noreferrer"
                    className={styles.certificateLink}
                  >
                    📄 Pobierz
                  </a>
                </td>
                <td className={styles.tableCell}>
                  <span
                    className={`${styles.badge} ${
                      attempt.isAccepted
                        ? styles.badgeAccepted
                        : styles.badgePending
                    }`}
                  >
                    {attempt.isAccepted ? "Zaakceptowany" : "Oczekuje"}
                  </span>
                </td>
                <td className={styles.tableCell}>
                  {!attempt.isAccepted && (
                    <div className={styles.actionButtons}>
                      <button
                        className={styles.acceptButton}
                        onClick={() => handleAccept(attempt.publicId)}
                        disabled={processingId === attempt.publicId}
                      >
                        {processingId === attempt.publicId ? "..." : "Akceptuj"}
                      </button>
                      <button
                        className={styles.declineButton}
                        onClick={() => handleDecline(attempt.publicId)}
                        disabled={processingId === attempt.publicId}
                      >
                        {processingId === attempt.publicId ? "..." : "Odrzuć"}
                      </button>
                    </div>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default TeacherRequests;
