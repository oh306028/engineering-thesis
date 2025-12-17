import React, { useState } from "react";
import styles from "../styles/AdminComponents.module.css";
import type { ClassroomsFilter } from "../adminService";

interface ClassFilterControlsProps {
  onFilterChange: (filter: ClassroomsFilter) => void;
  initialFilter: ClassroomsFilter;
}

const ClassFilter: React.FC<ClassFilterControlsProps> = ({
  onFilterChange,
  initialFilter,
}) => {
  const [currentFilter, setCurrentFilter] =
    useState<ClassroomsFilter>(initialFilter);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type } = e.target;

    let processedValue: string | Date | undefined = value;

    if (type === "date") {
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
    const emptyFilter: ClassroomsFilter = {
      className: undefined,
      classKey: undefined,
      dateCreatedFrom: undefined,
      dateCreatedTo: undefined,
      teacherName: undefined,
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
        <label className={styles.filterLabel} htmlFor="dateCreatedFrom">
          Utworzono OD
        </label>
        <input
          id="dateCreatedFrom"
          name="dateCreatedFrom"
          type="date"
          className={styles.filterInput}
          value={formatDateForInput(currentFilter.dateCreatedFrom)}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="dateCreatedTo">
          Utworzono DO
        </label>
        <input
          id="dateCreatedTo"
          name="dateCreatedTo"
          type="date"
          className={styles.filterInput}
          value={formatDateForInput(currentFilter.dateCreatedTo)}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="className">
          Nazwa Klasy
        </label>
        <input
          id="className"
          name="className"
          type="text"
          className={styles.filterInput}
          value={currentFilter.className || ""}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="teacherName">
          Nauczyciel
        </label>
        <input
          id="teacherName"
          name="teacherName"
          type="text"
          className={styles.filterInput}
          value={currentFilter.teacherName || ""}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="classKey">
          Kod Klasy
        </label>
        <input
          id="classKey"
          name="classKey"
          type="text"
          className={styles.filterInput}
          value={currentFilter.classKey || ""}
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

export default ClassFilter;
