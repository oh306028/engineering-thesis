import React, { useEffect, useState } from "react";
import styles from "../styles/StudentComponents.module.css";
import ClassroomService, {
  type ClassroomDetails,
} from "../../classroom/ClassroomService.tsx";
import NotificationService, {
  type MessageType,
  type NotificationModel,
} from "../../notification/NotificationService.tsx";

function Contact() {
  const [classroom, setClassroom] = useState<ClassroomDetails | null>(null);
  const [messages, setMessages] = useState<MessageType[]>([]);
  const [selectedMessage, setSelectedMessage] = useState<string>("");
  const [loading, setLoading] = useState(true);
  const [sending, setSending] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const [classroomData, messagesData] = await Promise.all([
        ClassroomService.GetMineClassroom(),
        NotificationService.GetMessagesDictionary(),
      ]);

      setClassroom(classroomData);
      setMessages(messagesData);
    } catch (error) {
      console.error("Błąd podczas pobierania danych:", error);
      setError("Nie udało się załadować danych");
    } finally {
      setLoading(false);
    }
  };

  const handleSendNotification = async () => {
    if (!selectedMessage) {
      setError("Wybierz wiadomość do wysłania");
      return;
    }

    if (!classroom?.teacherPublicId) {
      setError("Nie można wysłać wiadomości - brak nauczyciela");
      return;
    }

    setSending(true);
    setError("");
    setSuccess(false);

    try {
      const notification: NotificationModel = {
        userToId: classroom.teacherPublicId,
        messageId: selectedMessage,
      };

      await NotificationService.SendNotification(notification);
      setSuccess(true);
      setSelectedMessage("");

      setTimeout(() => setSuccess(false), 3000);
    } catch (error) {
      console.error("Błąd podczas wysyłania powiadomienia:", error);
      setError("Nie udało się wysłać wiadomości. Spróbuj ponownie.");
    } finally {
      setSending(false);
    }
  };

  if (loading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie...</p>
      </div>
    );
  }

  if (!classroom) {
    return (
      <div className={styles.contentSection}>
        <div className={styles.header}>
          <h2 className={styles.title}>
            <span className={styles.icon}>📩</span>
            Kontakt z Nauczycielem
          </h2>
          <p className={styles.subtitle}>
            Wyślij szybką wiadomość do swojego nauczyciela
          </p>
        </div>

        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>🔒</span>
          <p>Musisz dołączyć do klasy</p>
          <p className={styles.subtitle}>
            Aby skontaktować się z nauczycielem, najpierw dołącz do klasy w
            zakładce "Ranking uczniów"
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>📩</span>
          Kontakt z Nauczycielem
        </h2>
        <p className={styles.subtitle}>
          Wyślij szybką wiadomość do {classroom.teacherName}
        </p>
      </div>

      <div className={styles.pointsCard}>
        <div className={styles.pointsContent}>
          <span className={styles.pointsIcon}>👨‍🏫</span>
          <div>
            <div className={styles.pointsLabel}>Twój nauczyciel</div>
            <div className={styles.pointsValue} style={{ fontSize: "1.8rem" }}>
              {classroom.teacherName}
            </div>
          </div>
        </div>
        <div className={styles.levelBadge}>
          <span className={styles.levelIcon}>📧</span>
          <span>Wiadomości</span>
        </div>
      </div>

      {success && (
        <div
          className={styles.card}
          style={{
            background: "linear-gradient(135deg, #10b981 0%, #059669 100%)",
            color: "white",
            marginBottom: "24px",
          }}
        >
          <div
            style={{
              display: "flex",
              alignItems: "center",
              gap: "12px",
              padding: "8px",
            }}
          >
            <span style={{ fontSize: "2rem" }}>✅</span>
            <div>
              <div style={{ fontWeight: 700, fontSize: "1.1rem" }}>
                Wiadomość wysłana!
              </div>
              <div style={{ opacity: 0.9, fontSize: "0.9rem" }}>
                Nauczyciel otrzymał Twoją wiadomość
              </div>
            </div>
          </div>
        </div>
      )}

      {error && (
        <div
          className={styles.card}
          style={{
            background: "linear-gradient(135deg, #ef4444 0%, #dc2626 100%)",
            color: "white",
            marginBottom: "24px",
          }}
        >
          <div
            style={{
              display: "flex",
              alignItems: "center",
              gap: "12px",
              padding: "8px",
            }}
          >
            <span style={{ fontSize: "2rem" }}>⚠️</span>
            <div>
              <div style={{ fontWeight: 700, fontSize: "1.1rem" }}>Błąd</div>
              <div style={{ opacity: 0.9, fontSize: "0.9rem" }}>{error}</div>
            </div>
          </div>
        </div>
      )}

      <div className={styles.achievementsSection}>
        <h3 className={styles.sectionTitle}>Wybierz wiadomość</h3>

        {messages.length === 0 ? (
          <div className={styles.emptyState}>
            <span className={styles.emptyIcon}>📭</span>
            <p>Brak dostępnych wiadomości</p>
          </div>
        ) : (
          <div className={styles.cardsGrid}>
            {messages.map((message) => (
              <div
                key={message.messageId}
                className={`${styles.card} ${
                  selectedMessage === message.messageId
                    ? styles.cardSelected
                    : ""
                }`}
                onClick={() => setSelectedMessage(message.messageId)}
                style={{
                  cursor: "pointer",
                  border:
                    selectedMessage === message.messageId
                      ? "3px solid #5b4fc4"
                      : "2px solid #e5e7eb",
                  background:
                    selectedMessage === message.messageId
                      ? "linear-gradient(135deg, rgba(91, 79, 196, 0.1) 0%, rgba(124, 58, 237, 0.1) 100%)"
                      : "linear-gradient(135deg, #f9fafb 0%, #ffffff 100%)",
                }}
              >
                <div className={styles.cardHeader}>
                  <span className={styles.cardIcon}>
                    {selectedMessage === message.messageId ? "✅" : "💬"}
                  </span>
                </div>
                <p className={styles.cardDescription}>{message.message}</p>
              </div>
            ))}
          </div>
        )}
      </div>

      {messages.length > 0 && (
        <div style={{ marginTop: "32px", textAlign: "center" }}>
          <button
            className={styles.primaryButton}
            onClick={handleSendNotification}
            disabled={!selectedMessage || sending}
            style={{
              maxWidth: "400px",
              opacity: !selectedMessage || sending ? 0.5 : 1,
              cursor: !selectedMessage || sending ? "not-allowed" : "pointer",
            }}
          >
            {sending ? "Wysyłanie..." : "Wyślij wiadomość do nauczyciela"}
          </button>
          {!selectedMessage && (
            <p
              style={{
                marginTop: "12px",
                color: "#6b7280",
                fontSize: "0.9rem",
                fontWeight: 600,
              }}
            >
              Wybierz wiadomość, którą chcesz wysłać
            </p>
          )}
        </div>
      )}
    </div>
  );
}

export default Contact;
