import React, { useEffect, useState, useCallback } from "react";
import styles from "../styles/AdminComponents.module.css";
import AdminService, {
  type LogginHistoryListModel,
  type PaginationResult,
  type LogginsFilter,
  type PaginationEntry,
} from "../adminService.tsx";
import FilterControls from "./LogginFilter.tsx";
import Pagination from "../../../components/Pagination.tsx";

const DEFAULT_FILTER: LogginsFilter = {};
const DEFAULT_PAGINATION: PaginationEntry = { pageNumber: 1, pageSize: 10 };

function AdminMonitoring() {
  const [pagedLogins, setPagedLogins] = useState<
    PaginationResult<LogginHistoryListModel> | undefined
  >(undefined);
  const [loading, setLoading] = useState(true);

  const [filterState, setFilterState] = useState<LogginsFilter>(DEFAULT_FILTER);
  const [paginationState, setPaginationState] =
    useState<PaginationEntry>(DEFAULT_PAGINATION);

  const fetchLogins = useCallback(async () => {
    setLoading(true);
    try {
      const data = await AdminService.GetAllLoggins(
        filterState,
        paginationState
      );
      setPagedLogins(data);
    } catch (error) {
      console.error("Błąd podczas pobierania historii logowań:", error);
    } finally {
      setLoading(false);
    }
  }, [filterState, paginationState]);

  useEffect(() => {
    fetchLogins();
  }, [fetchLogins]);

  const handleFilterChange = (newFilter: LogginsFilter) => {
    setFilterState(newFilter);
    setPaginationState((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handlePageChange = (newPageNumber: number) => {
    setPaginationState((prev) => ({ ...prev, pageNumber: newPageNumber }));
  };

  const handlePageSizeChange = (newPageSize: number) => {
    setPaginationState((prev) => ({ ...prev, pageSize: newPageSize }));
    setPaginationState((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const logins = pagedLogins ? pagedLogins.items : [];
  const successCount = logins.filter((l) => l.isSucceeded).length;
  const failedCount = logins.filter((l) => !l.isSucceeded).length;

  const formatDateTime = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleString("pl-PL", {
      day: "numeric",
      month: "long",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  if (loading && !pagedLogins) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie danych monitoringu...</p>
      </div>
    );
  }

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>📊</span>
          Monitoring Systemu
        </h2>
        <p className={styles.subtitle}>
          Historia logowań i aktywność użytkowników
        </p>
      </div>

      <div className={styles.statsCard}>
        <div className={styles.statsContent}>
          <span className={styles.statsIcon}>📈</span>
          <div>
            <div className={styles.statsLabel}>Liczba logowań (Strona)</div>
            <div className={styles.statsValue}>{logins.length}</div>
          </div>
        </div>
        <div style={{ display: "flex", gap: "20px" }}>
          <div style={{ textAlign: "center" }}>
            <div
              style={{ fontSize: "2rem", fontWeight: 800, color: "#ffffff" }}
            >
              {successCount}
            </div>
            <div
              style={{
                fontSize: "0.9rem",
                color: "rgba(255, 255, 255, 0.9)",
                fontWeight: 600,
              }}
            >
              Udane
            </div>
          </div>
          <div style={{ textAlign: "center" }}>
            <div
              style={{ fontSize: "2rem", fontWeight: 800, color: "#ffffff" }}
            >
              {failedCount}
            </div>
            <div
              style={{
                fontSize: "0.9rem",
                color: "rgba(255, 255, 255, 0.9)",
                fontWeight: 600,
              }}
            >
              Nieudane
            </div>
          </div>
        </div>
      </div>

      <div className={styles.controlsContainer}>
        <FilterControls
          onFilterChange={handleFilterChange}
          initialFilter={filterState}
        />
      </div>

      {logins.length === 0 && !loading ? (
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>🔍</span>
          <p>Brak logowań spełniających kryteria.</p>
        </div>
      ) : (
        <>
          <table className={styles.table}>
            <thead className={styles.tableHeader}>
              <tr>
                <th>Email użytkownika</th>
                <th>Data i godzina</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {logins.map((login, index) => (
                <tr key={index} className={styles.tableRow}>
                  <td className={styles.tableCell}>{login.login}</td>
                  <td className={styles.tableCell}>
                    <span className={styles.dateText}>
                      {formatDateTime(login.loginDate)}
                    </span>
                  </td>
                  <td className={styles.tableCell}>
                    <span
                      className={`${styles.badge} ${
                        login.isSucceeded
                          ? styles.badgeSuccess
                          : styles.badgeFailed
                      }`}
                    >
                      {login.isSucceeded ? "Udane" : "Nieudane"}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div
            style={{
              display: "flex",
              justifyContent: "flex-end",
              marginTop: "20px",
            }}
          >
            <Pagination
              paginationResult={pagedLogins}
              paginationEntry={paginationState}
              onPageSizeChange={handlePageSizeChange}
              onPageChange={handlePageChange}
            />
          </div>
        </>
      )}
      {loading && pagedLogins && (
        <p style={{ textAlign: "center", marginTop: "20px", color: "#5b4fc4" }}>
          Pobieranie nowej strony...
        </p>
      )}
    </div>
  );
}

export default AdminMonitoring;
