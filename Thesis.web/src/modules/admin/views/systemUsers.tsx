import React, { useEffect, useState, useCallback } from "react";
import styles from "../styles/AdminComponents.module.css";
import AdminService, {
  type UserListModel,
  type UsersFilter,
  type PaginationEntry,
  type PaginationResult,
} from "../adminService.tsx";
import UserFilterControls from "./UserFilter";
import Pagination from "../../../components/Pagination.tsx";

// Domyślne stany początkowe
const DEFAULT_FILTER: UsersFilter = {};
const DEFAULT_PAGINATION: PaginationEntry = { pageNumber: 1, pageSize: 10 };

function SystemUsers() {
  // Stan przechowuje CAŁY wynik paginacji z API
  const [pagedUsers, setPagedUsers] = useState<
    PaginationResult<UserListModel> | undefined
  >(undefined);
  const [loading, setLoading] = useState(true);

  const [filterState, setFilterState] = useState<UsersFilter>(DEFAULT_FILTER);
  const [paginationState, setPaginationState] =
    useState<PaginationEntry>(DEFAULT_PAGINATION);

  const fetchUsers = useCallback(async () => {
    setLoading(true);
    try {
      const data = await AdminService.GetAllUsers(filterState, paginationState);
      setPagedUsers(data);
    } catch (error) {
      console.error("Błąd podczas pobierania użytkowników:", error);
    } finally {
      setLoading(false);
    }
  }, [filterState, paginationState]);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  const handleFilterChange = (newFilter: UsersFilter) => {
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

  const getRoleBadgeClass = (role: string) => {
    switch (role?.toLowerCase()) {
      case "student":
        return styles.badgeStudent;
      case "teacher":
        return styles.badgeTeacher;
      case "parent":
        return styles.badgeParent;
      default:
        return styles.badgePending;
    }
  };

  const getRoleLabel = (role: string) => {
    switch (role?.toLowerCase()) {
      case "student":
        return "Uczeń";
      case "teacher":
        return "Nauczyciel";
      case "parent":
        return "Rodzic";
      default:
        return role;
    }
  };

  if (loading && !pagedUsers) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie użytkowników...</p>
      </div>
    );
  }

  const users = pagedUsers ? pagedUsers.items : [];

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>👥</span>
          Użytkownicy Systemu
        </h2>
        <p className={styles.subtitle}>
          Przeglądaj wszystkich użytkowników w systemie
        </p>
      </div>

      <div className={styles.statsCard}>
        <div className={styles.statsContent}>
          <span className={styles.statsIcon}>👥</span>
          <div>
            <div className={styles.statsLabel}>
              Liczba użytkowników (Strona)
            </div>
            <div className={styles.statsValue}>{users.length}</div>
          </div>
        </div>
      </div>

      <div className={styles.controlsContainer}>
        <UserFilterControls
          onFilterChange={handleFilterChange}
          initialFilter={filterState}
        />
      </div>

      {users.length === 0 && !loading ? (
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>👤</span>
          <p>Brak użytkowników spełniających kryteria.</p>
        </div>
      ) : (
        <>
          <table className={styles.table}>
            <thead className={styles.tableHeader}>
              <tr>
                <th>Imię i nazwisko</th>
                <th>Login</th>
                <th>Email</th>
                <th>Rola</th>
              </tr>
            </thead>
            <tbody>
              {users.map((user, index) => (
                <tr key={index} className={styles.tableRow}>
                  <td className={styles.tableCell}>
                    {`${user.fullName ?? ""}`.trim() || "-"}
                  </td>
                  <td className={styles.tableCell}>{user.login ?? "-"}</td>
                  <td className={styles.tableCell}>{user.email ?? "-"}</td>
                  <td className={styles.tableCell}>
                    <span
                      className={`${styles.badge} ${getRoleBadgeClass(
                        user.role
                      )}`}
                    >
                      {getRoleLabel(user.role)}
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
              paginationResult={pagedUsers}
              paginationEntry={paginationState}
              onPageSizeChange={handlePageSizeChange}
              onPageChange={handlePageChange}
            />
          </div>
        </>
      )}
      {loading && pagedUsers && (
        <p style={{ textAlign: "center", marginTop: "20px", color: "#5b4fc4" }}>
          Pobieranie nowej strony...
        </p>
      )}
    </div>
  );
}

export default SystemUsers;
