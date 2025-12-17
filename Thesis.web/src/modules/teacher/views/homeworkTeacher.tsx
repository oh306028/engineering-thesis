import React, { useEffect, useState } from "react";
import HomeworkService, {
  type HomeWorkTypePair,
  type HomeWorkModel,
  type ExerciseHomeWorkDetails,
} from "../../classroom/ClassroomService.tsx";
import styles from "../styles/TeacherHomework.module.css";

interface Props {
  classId: string | null;
}

const HomeWorkTeacher: React.FC<Props> = ({ classId }) => {
  const [title, setTitle] = useState("");
  const [subject, setSubject] = useState("");
  const [description, setDescription] = useState("");
  const [deadline, setDeadline] = useState(null);
  const [type, setType] = useState("");
  const [types, setTypes] = useState<HomeWorkTypePair[]>([]);
  const [exercises, setExercises] = useState<ExerciseHomeWorkDetails[]>([
    { content: "" },
  ]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchTypes = async () => {
      try {
        const data = await HomeworkService.GetHomeworkTypesDictionary();
        setTypes(data);
        if (data.length > 0) setType(data[0].key);
      } catch (err) {
        console.error(err);
      }
    };
    fetchTypes();
  }, []);

  const handleExerciseChange = (index: number, value: string) => {
    const newExercises = [...exercises];
    newExercises[index].content = value;
    setExercises(newExercises);
  };

  const addExercise = () => {
    setExercises([...exercises, { content: "" }]);
  };

  const removeExercise = (index: number) => {
    const newExercises = exercises.filter((_, i) => i !== index);
    setExercises(newExercises);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setError("");

    const model: HomeWorkModel = {
      title,
      subject,
      description,
      deadline,
      type,
      exercises,
    };

    try {
      await HomeworkService.CreateHomeWorkExercise(classId!, model);
      alert("Zadanie domowe utworzone!");
      setTitle("");
      setSubject("");
      setDescription("");
      setDeadline(null);
      setExercises([{ content: "" }]);
      if (types.length > 0) setType(types[0].key);
    } catch (err) {
      console.error(err);
      setError("Wystąpił błąd podczas tworzenia zadania.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className={styles.page}>
      <h1 className={styles.title}>Tworzenie zadania domowego</h1>
      {error && <div className={styles.error}>{error}</div>}
      <form onSubmit={handleSubmit} className={styles.form}>
        <div className={styles.formGroup}>
          <label>Tytuł</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className={styles.input}
          />
        </div>

        <div className={styles.formGroup}>
          <label>Przedmiot</label>
          <input
            type="text"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            className={styles.input}
          />
        </div>

        <div className={styles.formGroup}>
          <label>Opis</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className={styles.input}
            rows={4}
          />
        </div>

        <div className={styles.formGroup}>
          <label>Termin</label>
          <input
            type="date"
            value={deadline}
            onChange={(e) => setDeadline(e.target.value)}
            className={styles.input}
          />
        </div>

        <div className={styles.formGroup}>
          <label>Typ zadania</label>
          <select
            value={type}
            onChange={(e) => setType(e.target.value)}
            className={styles.input}
          >
            {types.map((t) => (
              <option key={t.key} value={t.key}>
                {t.value}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.formGroup}>
          <label>Ćwiczenia</label>
          {exercises.map((exercise, index) => (
            <div key={index} className={styles.exerciseRow}>
              <input
                type="text"
                value={exercise.content}
                onChange={(e) => handleExerciseChange(index, e.target.value)}
                className={styles.input}
                placeholder={`Ćwiczenie ${index + 1}`}
              />
              <button
                type="button"
                className={styles.removeButton}
                onClick={() => removeExercise(index)}
                disabled={exercises.length === 1}
              >
                ❌
              </button>
            </div>
          ))}
          <button
            type="button"
            className={styles.addButton}
            onClick={addExercise}
          >
            ➕ Dodaj ćwiczenie
          </button>
        </div>

        <button
          type="submit"
          disabled={isSubmitting}
          className={styles.submitButton}
        >
          {isSubmitting ? "Tworzenie..." : "Utwórz zadanie"}
        </button>
      </form>
    </div>
  );
};

export default HomeWorkTeacher;
