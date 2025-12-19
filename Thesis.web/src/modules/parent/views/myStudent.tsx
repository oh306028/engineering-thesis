import React, { useEffect, useState } from "react";
import styles from "../styles/myStudent.module.css";
import { type StudentDetailsWithClassroom } from "../../classroom/ClassroomService";
import DictionaryService, {
  type KeyValuePair,
} from "../../../components/DictionaryService";
import StudentService, {
  type StudentFilterModel,
} from "../../student/studentService";
import { toast } from "react-toastify";

interface Props {
  student: StudentDetailsWithClassroom | undefined;
}

const DIFFICULTY_LEVELS = [
  { key: 0, value: "Poziom 0" },
  { key: 1, value: "Poziom 1" },
  { key: 2, value: "Poziom 2" },
  { key: 3, value: "Poziom 3" },
];

export default function MyStudent({ student }: Props) {
  const [subjects, setSubjects] = useState<KeyValuePair[]>([]);
  const [selectedSubject, setSelectedSubject] = useState<string>("");
  const [selectedLevel, setSelectedLevel] = useState<number>(0);
  const [isUpdating, setIsUpdating] = useState(false);

  useEffect(() => {
    const fetchSubjects = async () => {
      try {
        const data = await DictionaryService.GetSubjects();
        setSubjects(data);
      } catch (error) {
        console.error("Error fetching subjects:", error);
      }
    };
    fetchSubjects();
  }, []);

  useEffect(() => {
    if (student) {
      if (student.filterLevel !== undefined && student.filterLevel !== null) {
        setSelectedLevel(student.filterLevel);
      }
      if (student.filterSubject) {
        setSelectedSubject(student.filterSubject);
      }
    }
  }, [student]);

  const handleSaveFilters = async () => {
    if (!selectedSubject) return;

    setIsUpdating(true);
    try {
      const model: StudentFilterModel = {
        level: selectedLevel,
        subjectId: selectedSubject,
      };
      await StudentService.SetStudentLearningFilters(model);

      toast.success("Zapisano ustawienia nauki!", {
        position: "bottom-center",
        autoClose: 10000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        className: "custom-toast",
      });
    } catch (error) {
      console.error("Error updating filters:", error);
    } finally {
      setIsUpdating(false);
    }
  };

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
      <div className={styles.titleHeader}>
        <span className={styles.titleIcon}>👨‍🎓</span>
        <h2 className={styles.titleText}>Twój Uczeń</h2>
      </div>

      <p className={styles.description}>
        Poniżej znajdują się najważniejsze informacje dotyczące ucznia oraz
        konfiguracja nauki.
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

      <div className={`${styles.card} ${styles.filterSection}`}>
        <h3 className={styles.cardTitle}>⚙️ Ustawienia nauki</h3>
        <p className={styles.description}>
          Poniżej znajdują się ustawienia nauki Twojego ucznia. Ustaw przedmiot
          oraz poziom trudności (gdzie 0 oznacza najprostrze zadania), aby dziś
          Twój uczeń rozwiązywał takie typy zadań.
        </p>

        <div className={styles.filterGroup}>
          <label className={styles.label}>Wybierz przedmiot:</label>
          <select
            className={styles.select}
            value={selectedSubject}
            onChange={(e) => setSelectedSubject(e.target.value)}
          >
            <option value="">-- Wybierz przedmiot --</option>
            {subjects.map((s) => (
              <option key={s.key} value={s.key}>
                {s.value}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.filterGroup}>
          <label className={styles.label}>Poziom trudności:</label>
          <select
            className={styles.select}
            value={selectedLevel}
            onChange={(e) => setSelectedLevel(Number(e.target.value))}
          >
            {DIFFICULTY_LEVELS.map((l) => (
              <option key={l.key} value={l.key}>
                {l.value}
              </option>
            ))}
          </select>
        </div>

        <button
          className={styles.saveButton}
          onClick={handleSaveFilters}
          disabled={isUpdating || !selectedSubject}
        >
          {isUpdating ? "Zapisywanie..." : "Zapisz ustawienia"}
        </button>
      </div>
    </div>
  );
}
