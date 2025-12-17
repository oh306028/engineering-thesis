import React, { useState } from "react";
import styles from "../styles/AdminComponents.module.css";
import type { LogginsFilter } from "../adminService";

interface FilterControlsProps {
  onFilterChange: (filter: LogginsFilter) => void;
  initialFilter: LogginsFilter;
}

const FilterControls: React.FC<FilterControlsProps> = ({
  onFilterChange,
  initialFilter,
}) => {
  const [currentFilter, setCurrentFilter] =
    useState<LogginsFilter>(initialFilter);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;

    let processedValue: string | boolean | Date | undefined = value;

    if (name === "isSucceeded") {
      processedValue =
        value === "true" ? true : value === "false" ? false : undefined;
    } else if (name.startsWith("loginDate")) {
      processedValue = value ? new Date(value) : undefined;
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
    const emptyFilter: LogginsFilter = {
      loginDateFrom: undefined,
      loginDateTo: undefined,
      isSucceeded: undefined,
    };
    setCurrentFilter(emptyFilter);
    onFilterChange(emptyFilter);
  };

  const formatDateForInput = (date: Date | undefined): string => {
    if (!date) return "";
    return date.toISOString().split("T")[0];
  };

  return (
    <form className={styles.filterForm} onSubmit={handleSubmit}>
      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="loginDateFrom">
          Data logowania OD
        </label>
        <input
          id="loginDateFrom"
          name="loginDateFrom"
          type="date"
          className={styles.filterInput}
          value={formatDateForInput(currentFilter.loginDateFrom)}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="loginDateTo">
          Data logowania DO
        </label>
        <input
          id="loginDateTo"
          name="loginDateTo"
          type="date"
          className={styles.filterInput}
          value={formatDateForInput(currentFilter.loginDateTo)}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="isSucceeded">
          Status
        </label>
        <select
          id="isSucceeded"
          name="isSucceeded"
          className={styles.filterSelect}
          value={
            currentFilter.isSucceeded === true
              ? "true"
              : currentFilter.isSucceeded === false
              ? "false"
              : ""
          }
          onChange={handleChange}
        >
          <option value="">Wszystkie</option>
          <option value="true">Udane</option>
          <option value="false">Nieudane</option>
        </select>
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

export default FilterControls;
