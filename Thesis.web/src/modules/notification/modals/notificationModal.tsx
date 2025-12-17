import React, { useEffect, useState } from "react";
import styles from "../styles/notificationModal.module.css";
import NotificationService from "../NotificationService.tsx";
import type { NotificationDetails } from "../NotificationService";

interface NotificationModalProps {
  isOpen: boolean;
  onClose: () => void;
}

function NotificationModal({ isOpen, onClose }: NotificationModalProps) {
  const [notifications, setNotifications] = useState<NotificationDetails[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchNotifications = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await NotificationService.GetCurrentUserNotifications();
      const sortedData = data.sort(
        (a, b) =>
          new Date(b.notificationDate).getTime() -
          new Date(a.notificationDate).getTime()
      );
      setNotifications(sortedData);
    } catch (err) {
      console.error("Błąd podczas ładowania powiadomień:", err);
      setError("Nie udało się załadować powiadomień.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (isOpen) {
      fetchNotifications();
    }
  }, [isOpen]);

  if (!isOpen) return null;

  const handleBackdropClick = (e: React.MouseEvent) => {
    if (e.target === e.currentTarget) {
      onClose();
    }
  };


  const formatNotificationDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("pl-PL", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const getNotificationIcon = (type: string, isSystem: boolean) => {
    if (isSystem) return "🤖";
    switch (type.toLowerCase()) {
      case "homework_assigned":
        return "📝";
      case "achievement_unlocked":
        return "🏆";
      case "message":
        return "✉️";
      case "level_up":
        return "🚀";
      default:
        return "💡";
    }
  };

  return (
    <div className={styles.backdrop} onClick={handleBackdropClick}>
      <div className={styles.modal}>
        <button className={styles.closeButton} onClick={onClose}>
          ✕
        </button>

        <div className={styles.modalHeader}>
          <h2 className={styles.modalTitle}>🔔 Twoje Powiadomienia</h2>
          <p className={styles.modalSubtitle}>
            Ostatnia aktywność i wiadomości.
          </p>
        </div>

        <div className={styles.modalContent}>
          {isLoading && <div className={styles.loadingState}>Ładowanie...</div>}
          {error && <div className={styles.errorText}>Błąd: {error}</div>}
          {!isLoading && !error && (
            <>
              {notifications.length === 0 ? (
                <div className={styles.emptyState}>
                  <span className={styles.emptyIcon}>✨</span>
                  <p>Brak nowych powiadomień.</p>
                </div>
              ) : (
                <ul className={styles.notificationList}>
                  {notifications.map((n) => (
                    <li
                      key={n.publicId}
                      className={`${styles.notificationItem} ${
                        n.isSeen ? styles.seen : styles.unseen
                      }`}
                    >
                      <span className={styles.notificationIcon}>
                        {getNotificationIcon(
                          n.notificationType,
                          n.isSystemNotification
                        )}
                      </span>
                      <div className={styles.notificationBody}>
                        <p className={styles.notificationMessage}>
                          {n.message}
                        </p>
                        <div className={styles.notificationMeta}>
                          <span>{n.notifiedBy}</span>
                          <span className={styles.date}>
                            {formatNotificationDate(n.notificationDate)}
                          </span>
                        </div>
                      </div>
                    </li>
                  ))}
                </ul>
              )}
            </>
          )}
        </div>

        <div className={styles.modalFooter}>
          <button className={styles.closeButtonFooter} onClick={onClose}>
            Zamknij
          </button>
        </div>
      </div>
    </div>
  );
}

export default NotificationModal;
