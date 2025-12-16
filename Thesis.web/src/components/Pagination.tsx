import React from "react";
import styles from "./Pagination.module.css";
import type {
  PaginationResult,
  PaginationEntry,
} from "../modules/admin/adminService";

interface PaginationControlsProps {
  paginationResult: PaginationResult<any> | undefined;
  paginationEntry: PaginationEntry;
  onPageSizeChange: (pageSize: number) => void;
  onPageChange: (pageNumber: number) => void;
}

const PAGE_SIZE_OPTIONS = [10, 20, 50];

const Pagination: React.FC<PaginationControlsProps> = ({
  paginationResult,
  paginationEntry,
  onPageSizeChange,
  onPageChange,
}) => {
  if (!paginationResult) {
    return null;
  }

  const { totalCount, pageNumber, pageSize, totalPages } = paginationResult;

  const handlePageSizeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    onPageSizeChange(parseInt(e.target.value, 10));
    onPageChange(1);
  };

  const handlePrevious = () => {
    if (pageNumber > 1) {
      onPageChange(pageNumber - 1);
    }
  };

  const handleNext = () => {
    if (pageNumber < totalPages) {
      onPageChange(pageNumber + 1);
    }
  };

  const isPrevDisabled = pageNumber <= 1;
  const isNextDisabled = pageNumber >= totalPages;

  return (
    <div className={styles.paginationContainer}>
      <span className={styles.paginationLabel}>Elementów na stronę:</span>
      <select
        className={styles.paginationSelect}
        value={pageSize}
        onChange={handlePageSizeChange}
      >
        {PAGE_SIZE_OPTIONS.map((size) => (
          <option key={size} value={size}>
            {size}
          </option>
        ))}
      </select>

      <span className={styles.paginationInfo}>
        Strona {pageNumber} z {totalPages} ({totalCount} rekordów)
      </span>

      <button
        onClick={handlePrevious}
        disabled={isPrevDisabled}
        className={styles.paginationButton}
      >
        &lsaquo; Poprzednia
      </button>
      <button
        onClick={handleNext}
        disabled={isNextDisabled}
        className={styles.paginationButton}
      >
        Następna &rsaquo;
      </button>
    </div>
  );
};

export default Pagination;
