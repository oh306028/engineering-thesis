import { useState, useEffect } from "react";
import DictionaryService, {
  type KeyValuePair,
} from "../../../components/DictionaryService";
import StudentService from "../../student/studentService";
import styles from "../styles/teacherDashboard.module.css";
import { toast } from "react-toastify";

const PathForm = ({
  onCancel,
  onSuccess,
}: {
  onCancel: () => void;
  onSuccess: () => void;
}) => {
  const [subjects, setSubjects] = useState<KeyValuePair[]>([]);
  const [badges, setBadges] = useState<KeyValuePair[]>([]);
  const [formData, setFormData] = useState({
    name: "",
    level: 1,
    subjectId: "",
    badgeId: "default",
  });

  useEffect(() => {
    DictionaryService.GetSubjects().then(setSubjects);
    DictionaryService.GetBadges().then(setBadges);
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await StudentService.CreatePath(formData);
    toast.success("Zapisano szkic ścieżki!", {
      position: "bottom-center",
      autoClose: 5000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      className: "custom-toast",
    });
    onSuccess();
  };

  return (
    <div className={styles.contentSection}>
      <h3>Nowa Ścieżka Nauki</h3>
      <form onSubmit={handleSubmit} className={styles.classCreatorForm}>
        <input
          className={styles.classCreatorInput}
          placeholder="Nazwa ścieżki"
          onChange={(e) => setFormData({ ...formData, name: e.target.value })}
          required
        />
        <select
          className={styles.classCreatorInput}
          onChange={(e) =>
            setFormData({ ...formData, subjectId: e.target.value })
          }
          required
        >
          <option value="">Wybierz przedmiot</option>
          {subjects.map((s) => (
            <option key={s.key} value={s.key}>
              {s.value}
            </option>
          ))}
        </select>
        <input
          type="number"
          min="0"
          max="4"
          step={1}
          className={styles.classCreatorInput}
          placeholder="Poziom (0-4)"
          onChange={(e) =>
            setFormData({ ...formData, level: parseInt(e.target.value) })
          }
        />
        <select
          className={styles.classCreatorInput}
          onChange={(e) =>
            setFormData({ ...formData, badgeId: e.target.value })
          }
          required
        >
          <option value="">Wybierz odznakę za ukończenie</option>
          {badges.map((s) => (
            <option key={s.key} value={s.key}>
              {s.value}
            </option>
          ))}
        </select>
        <div className={styles.classCreatorActions}>
          <button type="submit" className={styles.selectClassButton}>
            Zapisz Szkic
          </button>
          <button
            type="button"
            onClick={onCancel}
            className={styles.cancelButton}
          >
            Anuluj
          </button>
        </div>
      </form>
    </div>
  );
};

export default PathForm;
