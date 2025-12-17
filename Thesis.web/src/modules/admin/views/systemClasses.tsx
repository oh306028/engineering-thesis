import React, { useEffect, useState, useCallback } from "react";
import styles from "../styles/AdminComponents.module.css";
import AdminService, {
  type ClassroomListModel,
  type ClassroomsFilter,
  type PaginationEntry,
  type PaginationResult,
} from "../adminService.tsx";
import Pagination from "../../../components/Pagination.tsx";
import ClassFilter from "./ClassFilter.tsx";

const DEFAULT_FILTER: ClassroomsFilter = {};
const DEFAULT_PAGINATION: PaginationEntry = { pageNumber: 1, pageSize: 10 };

function SystemClasses() {
  const [pagedClasses, setPagedClasses] = useState<
    PaginationResult<ClassroomListModel> | undefined
  >(undefined);
  const [loading, setLoading] = useState(true);

  const [filterState, setFilterState] =
    useState<ClassroomsFilter>(DEFAULT_FILTER);
  const [paginationState, setPaginationState] =
    useState<PaginationEntry>(DEFAULT_PAGINATION);

  const fetchClasses = useCallback(async () => {
    setLoading(true);
    try {
      const data = await AdminService.GetAllClasses(
        filterState,
        paginationState
      );
      setPagedClasses(data);
    } catch (error) {
      console.error("Błąd podczas pobierania klas:", error);
    } finally {
      setLoading(false);
    }
  }, [filterState, paginationState]);

  useEffect(() => {
    fetchClasses();
  }, [fetchClasses]);

  const handleFilterChange = (newFilter: ClassroomsFilter) => {
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

  const formatDate = (dateString: string | Date) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("pl-PL", {
      day: "numeric",
      month: "long",
      year: "numeric",
    });
  };

  if (loading && !pagedClasses) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie klas...</p>
      </div>
    );
  }

  const classes = pagedClasses ? pagedClasses.items : [];

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>🏫</span>
          Klasy w Systemie
        </h2>
        <p className={styles.subtitle}>
          Przeglądaj wszystkie klasy utworzone w systemie
        </p>
      </div>

      <div className={styles.statsCard}>
        <div className={styles.statsContent}>
          <span className={styles.statsIcon}>🏫</span>
          <div>
            <div className={styles.statsLabel}>Liczba klas (Strona)</div>
            <div className={styles.statsValue}>{classes.length}</div>
          </div>
        </div>
      </div>
      <div className={styles.controlsContainer}>
        <ClassFilter
          onFilterChange={handleFilterChange}
          initialFilter={filterState}
        />
      </div>

      {classes.length === 0 && !loading ? (
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>🏫</span>
          <p>Brak klas spełniających kryteria.</p>
        </div>
      ) : (
        <table className={styles.table}>
          <thead className={styles.tableHeader}>
            <tr>
              <th>Nazwa klasy</th>
              <th>Nauczyciel</th>
              <th>Kod klasy</th>
              <th>Data utworzenia</th>
            </tr>
          </thead>
          <tbody>
            {classes.map((classroom, index) => (
              <tr key={index} className={styles.tableRow}>
                <td className={styles.tableCell}>{classroom.className}</td>
                <td className={styles.tableCell}>{classroom.teacherName}</td>
                <td className={styles.tableCell}>
                  <span className={`${styles.badge} ${styles.badgePending}`}>
                    {classroom.classroomKey}
                  </span>
                </td>
                <td className={styles.tableCell}>
                  <span className={styles.dateText}>
                    {formatDate(classroom.dateCreated)}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {pagedClasses && (
        <div
          style={{
            display: "flex",
            justifyContent: "flex-end",
            marginTop: "20px",
          }}
        >
          <Pagination
            paginationResult={pagedClasses}
            paginationEntry={paginationState}
            onPageSizeChange={handlePageSizeChange}
            onPageChange={handlePageChange}
          />
        </div>
      )}

      {loading && pagedClasses && (
        <p style={{ textAlign: "center", marginTop: "20px", color: "#5b4fc4" }}>
          Pobieranie nowej strony...
        </p>
      )}
    </div>
  );
}

export default SystemClasses;
