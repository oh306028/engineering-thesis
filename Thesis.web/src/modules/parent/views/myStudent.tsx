import React from "react";
import styles from "../styles/myStudent.module.css";
import { type StudentDetailsWithClassroom } from "../../classroom/ClassroomService";

interface Props {
  student: StudentDetailsWithClassroom | undefined;
}

export default function MyStudent({ student }: Props) {
  if (!student) {
    return (
      <div className={styles.studentContainer}>
        <div className={styles.titleHeader}>
          <span className={styles.titleIcon}>⏳</span>
          <h2 className={styles.titleText}>Wczytywanie danych...</h2>
        </div>
        <p className={styles.description}>
          Pobieramy informacje o Twoim uczniu.
        </p>
      </div>
    );
  }

  return (
    <div className={styles.studentContainer}>
      {/* Nagłówek */}
      <div className={styles.titleHeader}>
        <span className={styles.titleIcon}>👨‍🎓</span>
        <h2 className={styles.titleText}>Twój Uczeń</h2>
      </div>

      <p className={styles.description}>
        Poniżej znajdują się najważniejsze informacje dotyczące ucznia.
      </p>

      <div className={styles.card}>
        <h3 className={styles.cardTitle}>🎒 Informacje szczegółowe</h3>

        <p className={styles.infoRow}>
          <span className={styles.emoji}>🧑‍🏫</span>
          <strong>Imię i nazwisko:</strong> {student.name}
        </p>

        <p className={styles.infoRow}>
          <span className={styles.emoji}>📚</span>
          <strong>Poziom:</strong> {student.level}
        </p>

        <p className={styles.infoRow}>
          <span className={styles.emoji}>⭐</span>
          <strong>Punkty:</strong> {student.currentPoints}
        </p>

        <p className={styles.infoRow}>
          <span className={styles.emoji}>🏅</span>
          <strong>Odznaki:</strong> {student.badgesCount}
        </p>

        <p className={styles.infoRow}>
          <span className={styles.emoji}>👀</span>
          <strong>Ostatnia aktywność:</strong>{" "}
          {new Date(student.lastSeenAt).toLocaleString("pl-PL")}
        </p>
      </div>
    </div>
  );
}
