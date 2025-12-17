import React, { useEffect, useState, useCallback } from "react";
import styles from "../styles/notificationList.module.css";
import AdminComponentsStyles from "../../admin/styles/AdminComponents.module.css";

import NotificationService from "../NotificationService.tsx";
import type {
  NotificationDetails,
  NotificationListFilter,
} from "../NotificationService";
import Pagination from "../../../components/Pagination.tsx";
import type {
  PaginationEntry,
  PaginationResult,
} from "../../admin/AdminService.tsx";
import NotificationFilter from "./NotificationFilter.tsx";

interface Props {
  classId: string | null;
  fromParents?: boolean;
}

const DEFAULT_FILTER: NotificationListFilter = {};
const DEFAULT_PAGINATION: PaginationEntry = { pageNumber: 1, pageSize: 10 };

const NotificationList: React.FC<Props> = ({ classId, fromParents }) => {
  const [pagedNotifications, setPagedNotifications] = useState<
    PaginationResult<NotificationDetails> | undefined
  >(undefined);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isMarkingId, setIsMarkingId] = useState<string | null>(null);

  const [filterState, setFilterState] =
    useState<NotificationListFilter>(DEFAULT_FILTER);
  const [paginationState, setPaginationState] =
    useState<PaginationEntry>(DEFAULT_PAGINATION);

  const fetchNotifications = useCallback(async () => {
    if (!classId) return;

    setIsLoading(true);
    setError(null);
    try {
      var data: PaginationResult<NotificationDetails>;

      if (fromParents) {
        data = await NotificationService.GetNotificationsFromParents(
          filterState,
          paginationState,
          classId
        );
      } else {
        data = await NotificationService.GetNotificationsFromStudents(
          filterState,
          paginationState,
          classId
        );
      }

      const sortedItems = data.items.sort(
        (a, b) =>
          new Date(b.notificationDate).getTime() -
          new Date(a.notificationDate).getTime()
      );
      data.items = sortedItems;

      setPagedNotifications(data);
    } catch (err) {
      console.error("Błąd podczas ładowania powiadomień:", err);
      setError("Nie udało się załadować powiadomień.");
    } finally {
      setIsLoading(false);
    }
  }, [classId, fromParents, filterState, paginationState]);

  const handleMarkAsSeen = async (id: string) => {
    if (isLoading || isMarkingId) return;

    setIsMarkingId(id);
    try {
      await NotificationService.MarkAsSeen(id);

      setPagedNotifications((prev) => {
        if (!prev) return undefined;

        const updatedItems = prev.items.map((n) =>
          n.publicId === id ? { ...n, isSeen: true } : n
        );
        return { ...prev, items: updatedItems };
      });
    } catch (err) {
      console.error("Błąd podczas oznaczania powiadomienia:", err);
    } finally {
      setIsMarkingId(null);
      fetchNotifications();
    }
  };

  useEffect(() => {
    fetchNotifications();
  }, [fetchNotifications]);

  const handleFilterChange = (newFilter: NotificationListFilter) => {
    setFilterState(newFilter);
    setPaginationState((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handlePageChange = (newPageNumber: number) => {
    setPaginationState((prev) => ({ ...prev, pageNumber: newPageNumber }));
  };

  const handlePageSizeChange = (newPageSize: number) => {
    setPaginationState((prev) => ({
      ...prev,
      pageSize: newPageSize,
      pageNumber: 1,
    }));
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
    switch (type?.toLowerCase()) {
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

  const notifications = pagedNotifications ? pagedNotifications.items : [];
  const showLoadingOverlay = isLoading && !pagedNotifications;

  return (
    <div className={styles.wrapper}>
      <div className={styles.header}>
        <h2 className={styles.title}>🔔 Powiadomienia</h2>
        <p className={styles.subtitle}>Ostatnia aktywność i komunikaty.</p>
      </div>

      <div className={AdminComponentsStyles.controlsContainer}>
        <NotificationFilter
          onFilterChange={handleFilterChange}
          initialFilter={filterState}
        />
      </div>

      <div className={styles.content}>
        {showLoadingOverlay && (
          <div className={styles.loading}>Ładowanie...</div>
        )}
        {error && <div className={styles.error}>{error}</div>}

        {!showLoadingOverlay && !error && (
          <>
            {notifications.length === 0 ? (
              <div className={styles.empty}>
                <span className={styles.emptyIcon}>✨</span>
                <p>Brak powiadomień spełniających kryteria.</p>
              </div>
            ) : (
              <ul className={styles.list}>
                {notifications.map((n) => {
                  const isBeingMarked = isMarkingId === n.publicId;

                  return (
                    <li
                      key={n.publicId}
                      className={`${styles.item} ${
                        n.isSeen ? styles.seen : styles.unseen
                      }`}
                    >
                      <span className={styles.icon}>
                        {getNotificationIcon(
                          n.notificationType,
                          n.isSystemNotification
                        )}
                      </span>

                      <div className={styles.body}>
                        <p className={styles.message}>{n.message}</p>

                        <div className={styles.meta}>
                          <span className={styles.from}>{n.notifiedBy}</span>
                          <span className={styles.date}>
                            {formatNotificationDate(n.notificationDate)}
                          </span>
                        </div>
                      </div>

                      <div className={styles.actions}>
                        <button
                          onClick={() => handleMarkAsSeen(n.publicId)}
                          disabled={n.isSeen || isBeingMarked}
                          className={styles.actionButton}
                          title={
                            n.isSeen ? "Przeczytane" : "Oznacz jako przeczytane"
                          }
                        >
                          {isBeingMarked ? (
                            <span className={styles.spinnerSmall}>🔄</span>
                          ) : n.isSeen ? (
                            "✔"
                          ) : (
                            "👀📩"
                          )}
                        </button>
                      </div>
                    </li>
                  );
                })}
              </ul>
            )}

            {pagedNotifications && (
              <div
                style={{
                  display: "flex",
                  justifyContent: "flex-end",
                  marginTop: "20px",
                }}
              >
                <Pagination
                  paginationResult={pagedNotifications}
                  paginationEntry={paginationState}
                  onPageSizeChange={handlePageSizeChange}
                  onPageChange={handlePageChange}
                />
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
};

export default NotificationList;
