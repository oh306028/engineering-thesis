import React, { useState } from "react";
import styles from "../../admin/styles/AdminComponents.module.css";
import type { NotificationListFilter } from "../NotificationService";

interface NotificationFilterControlsProps {
  onFilterChange: (filter: NotificationListFilter) => void;
  initialFilter: NotificationListFilter;
}

const NotificationFilter: React.FC<NotificationFilterControlsProps> = ({
  onFilterChange,
  initialFilter,
}) => {
  const [currentFilter, setCurrentFilter] =
    useState<NotificationListFilter>(initialFilter);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value, type } = e.target;

    let processedValue: string | boolean | Date | undefined = value;

    if (name === "isSeen") {
      processedValue =
        value === "true" ? true : value === "false" ? false : undefined;
    } else if (name === "notificationDate") {
      processedValue = value ? new Date(value) : undefined;
    } else {
      processedValue = value === "" ? undefined : value;
    }

    setCurrentFilter((prev) => ({
      ...prev,
      [name]: processedValue,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onFilterChange(currentFilter);
  };

  const handleReset = () => {
    const emptyFilter: NotificationListFilter = {
      isSeen: undefined,
      notifiedBy: undefined,
      notificationDate: undefined,
    };
    setCurrentFilter(emptyFilter);
    onFilterChange(emptyFilter);
  };

  const formatDateForInput = (date: Date | undefined): string => {
    if (!date) return "";
    return date.toISOString().split("T")[0];
  };

  const selectedSeenStatus =
    currentFilter.isSeen === true
      ? "true"
      : currentFilter.isSeen === false
      ? "false"
      : "false";

  return (
    <form className={styles.filterForm} onSubmit={handleSubmit}>
      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="isSeen">
          Status
        </label>
        <select
          id="isSeen"
          name="isSeen"
          className={styles.filterSelect}
          value={selectedSeenStatus}
          onChange={handleChange}
        >
          <option value="false">Nieprzeczytane</option>
          <option value="true">Przeczytane</option>
        </select>
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="notificationDate">
          Data
        </label>
        <input
          id="notificationDate"
          name="notificationDate"
          type="date"
          className={styles.filterInput}
          value={formatDateForInput(currentFilter.notificationDate)}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="notifiedBy">
          Nadawca
        </label>
        <input
          id="notifiedBy"
          name="notifiedBy"
          type="text"
          className={styles.filterInput}
          value={currentFilter.notifiedBy || ""}
          onChange={handleChange}
        />
      </div>

      <button type="submit" className={styles.filterButton}>
        Filtruj
      </button>
      <button
        type="button"
        className={styles.filterButton}
        style={{ background: "#6b7280" }}
        onClick={handleReset}
      >
        Reset
      </button>
    </form>
  );
};

export default NotificationFilter;
