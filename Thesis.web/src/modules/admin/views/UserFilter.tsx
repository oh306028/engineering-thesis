import React, { useState } from "react";
import styles from "../styles/AdminComponents.module.css";
import type { UsersFilter } from "../adminService";

interface UserFilterControlsProps {
  onFilterChange: (filter: UsersFilter) => void;
  initialFilter: UsersFilter;
}

const ROLE_OPTIONS = [
  { label: "Wszystkie", value: "" },
  { label: "Nauczyciel", value: "Teacher" },
  { label: "Rodzic", value: "Parent" },
  { label: "Uczeń", value: "Student" },
];

const UserFilter: React.FC<UserFilterControlsProps> = ({
  onFilterChange,
  initialFilter,
}) => {
  const [currentFilter, setCurrentFilter] =
    useState<UsersFilter>(initialFilter);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;

    const processedValue = value === "" ? undefined : value;

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
    const emptyFilter: UsersFilter = {
      role: undefined,
      login: undefined,
      email: undefined,
      firstName: undefined,
      lastName: undefined,
    };
    setCurrentFilter(emptyFilter);
    onFilterChange(emptyFilter);
  };

  const selectedRole = currentFilter.role || "";

  return (
    <form className={styles.filterForm} onSubmit={handleSubmit}>
      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="role">
          Rola
        </label>
        <select
          id="role"
          name="role"
          className={styles.filterSelect}
          value={selectedRole}
          onChange={handleChange}
        >
          {ROLE_OPTIONS.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="lastName">
          Nazwisko
        </label>
        <input
          id="lastName"
          name="lastName"
          type="text"
          className={styles.filterInput}
          value={currentFilter.lastName || ""}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="login">
          Login
        </label>
        <input
          id="login"
          name="login"
          type="text"
          className={styles.filterInput}
          value={currentFilter.login || ""}
          onChange={handleChange}
        />
      </div>

      <div className={styles.filterGroup}>
        <label className={styles.filterLabel} htmlFor="email">
          Email
        </label>
        <input
          id="email"
          name="email"
          type="text"
          className={styles.filterInput}
          value={currentFilter.email || ""}
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

export default UserFilter;
