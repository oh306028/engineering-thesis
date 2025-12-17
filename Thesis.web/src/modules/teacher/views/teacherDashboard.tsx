import React, { useEffect, useState } from "react";
import NavBar from "../../../components/NavBar.tsx";
import styles from "../styles/teacherDashboard.module.css";
import ClassroomService, {
  type ClassroomDetails,
} from "../../classroom/ClassroomService.tsx";
import MyClass from "../../student/views/myClass.tsx";
import JoinRequestsView from "./joinRequests.tsx";
import NotificationList from "../../notification/views/notificationList.tsx";
import HomeWorkTeacher from "./homeworkTeacher.tsx";

type MenuItem =
  | "my-classes"
  | "join-requests"
  | "my-students"
  | "student-messages"
  | "parent-messages"
  | "homework";

export interface ClassroomCreateModel {
  className: string;
}

interface ActiveClassDetails extends ClassroomDetails {
  teacherName: string;
}

interface ClassCreatorProps {
  onCreate: (model: ClassroomCreateModel) => Promise<void>;
  onCancel: () => void;
}

const ClassCreator: React.FC<ClassCreatorProps> = ({ onCreate, onCancel }) => {
  const [className, setClassName] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!className.trim()) return;

    setIsLoading(true);
    try {
      await onCreate({ className: className.trim() });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles.contentSection}>
      <h2 className={styles.contentTitle}>🆕 Stwórz Nową Klasę</h2>
      <p className={styles.contentDescription}>
        Podaj nazwę klasy, którą chcesz utworzyć.
      </p>
      <form onSubmit={handleSubmit} className={styles.classCreatorForm}>
        <input
          type="text"
          value={className}
          onChange={(e) => setClassName(e.target.value)}
          placeholder="Np. 3A - Matematyka zaawansowana"
          className={styles.classCreatorInput}
          disabled={isLoading}
          required
        />
        <div className={styles.classCreatorActions}>
          <button
            type="submit"
            className={styles.selectClassButton}
            disabled={isLoading}
          >
            {isLoading ? "Tworzenie..." : "Utwórz Klasę"}
          </button>
          <button
            type="button"
            className={styles.cancelButton}
            onClick={onCancel}
            disabled={isLoading}
          >
            Anuluj
          </button>
        </div>
      </form>
    </div>
  );
};

interface ClassSelectorProps {
  classes: ClassroomDetails[];
  onClassSelected: (classDetails: ActiveClassDetails) => void;
  onSelectCreateNew: () => void;
}

const ClassSelector: React.FC<ClassSelectorProps> = ({
  classes,
  onClassSelected,
  onSelectCreateNew,
}) => {
  const teacherName = "Prof. Anna Kowalska (Demo)";

  const handleSelectClass = (classroom: ClassroomDetails) => {
    onClassSelected({
      ...classroom,
      teacherName: teacherName,
    });
  };

  return (
    <div className={styles.classSelectorContainer}>
      <div className={styles.contentSection}>
        <h2 className={styles.contentTitle}>
          <span className={styles.contentIcon}>🏫</span>
          Moje Klasy
        </h2>
        <p className={styles.contentDescription}>
          Wybierz klasę, aby zarządzać jej szczegółami.
        </p>

        {classes.length > 0 ? (
          <div className={styles.classList}>
            {classes.map((c) => (
              <button
                key={c.publicId}
                className={styles.classItemButton}
                onClick={() => handleSelectClass(c)}
              >
                {c.className}
              </button>
            ))}
          </div>
        ) : (
          <div className={styles.emptyClassList}>
            <p>Nie masz jeszcze żadnych klas.</p>
          </div>
        )}

        <button
          className={styles.selectClassButton}
          onClick={onSelectCreateNew}
        >
          + Stwórz Nową Klasę
        </button>
      </div>
    </div>
  );
};

function TeacherDashboard() {
  const [activeMenu, setActiveMenu] = useState<MenuItem>("my-classes");
  const [activeClass, setActiveClass] = useState<ActiveClassDetails | null>(
    null
  );
  const [classes, setClasses] = useState<ClassroomDetails[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isCreatingClass, setIsCreatingClass] = useState(false);

  const fetchTeacherClasses = async () => {
    setIsLoading(true);
    try {
      const response = await ClassroomService.GetList();
      setClasses(response);
    } catch (error) {
      console.error("Błąd podczas pobierania klas:", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchTeacherClasses();
  }, []);

  const handleClassCreate = async (model: ClassroomCreateModel) => {
    try {
      console.log("Tworzenie klasy z modelem:", model);

      setIsCreatingClass(false);
    } catch (error) {
      console.error("Błąd podczas tworzenia klasy:", error);
      throw error; // Propagacja błędu do ClassCreator
    }
  };

  const handleClassSelected = (classDetails: ActiveClassDetails) => {
    setActiveClass(classDetails);
    setActiveMenu("join-requests");
  };

  const clearActiveClass = () => {
    setActiveClass(null);
    setActiveMenu("my-classes");
    setIsCreatingClass(false);
  };

  const renderContent = () => {
    if (isLoading) {
      return (
        <div className={styles.loadingState}>
          <p>Ładowanie klas...</p>
        </div>
      );
    }

    if (isCreatingClass) {
      return (
        <ClassCreator
          onCreate={handleClassCreate}
          onCancel={() => setIsCreatingClass(false)}
        />
      );
    }

    if (!activeClass) {
      return (
        <ClassSelector
          classes={classes}
          onClassSelected={handleClassSelected}
          onSelectCreateNew={() => setIsCreatingClass(true)}
        />
      );
    }

    switch (activeMenu) {
      case "join-requests":
        return <JoinRequestsView classId={activeClass.publicId} />;
      case "my-students":
        return <MyClass publicId={activeClass.publicId} />;
      case "student-messages":
        return (
          <NotificationList
            classId={activeClass.publicId}
            fromParents={false}
          />
        );
      case "parent-messages":
        return (
          <NotificationList classId={activeClass.publicId} fromParents={true} />
        );
      case "homework":
        return <HomeWorkTeacher classId={activeClass.publicId} />;
      case "my-classes":
      default:
        return (
          <ClassSelector
            classes={classes}
            onClassSelected={handleClassSelected}
            onSelectCreateNew={() => setIsCreatingClass(true)}
          />
        );
    }
  };

  return (
    <div className={styles.page}>
      <NavBar />

      <div className={styles.dashboardContainer}>
        <aside className={styles.sidebar}>
          <div className={styles.sidebarHeader}>
            <h3 className={styles.sidebarTitle}>Dashboard Nauczyciela</h3>
            <div className={styles.userBadge}>
              <span className={styles.userIcon}>👩‍🏫</span>
            </div>

            {activeClass ? (
              <div className={styles.classInfoSection}>
                <div className={styles.classInfo}>
                  <span className={styles.classLabel}>Aktywna Klasa</span>
                  <span className={styles.className}>
                    {activeClass.className} - {activeClass.classroomKey}
                  </span>
                </div>
                <button
                  className={styles.changeClassButton}
                  onClick={clearActiveClass}
                >
                  Zmień Klasę
                </button>
              </div>
            ) : (
              <div className={styles.classInfoSection}>
                <p className={styles.classPrompt}>
                  Wybierz klasę, aby uzyskać dostęp do szczegółów.
                </p>
              </div>
            )}
          </div>

          <nav className={styles.navigation}>
            <div className={styles.navSection}>
              <h4 className={styles.navSectionTitle}>Główne</h4>
              <ul className={styles.navList}>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "my-classes" ? styles.navItemActive : ""
                    }`}
                    onClick={() => {
                      clearActiveClass();
                      setActiveMenu("my-classes");
                    }}
                  >
                    <span className={styles.navIcon}>🏫</span>
                    <span className={styles.navText}>Moje Klasy</span>
                  </button>
                </li>
              </ul>
            </div>

            {activeClass && (
              <div className={styles.navSection}>
                <h4 className={styles.navSectionTitle}>
                  Zarządzanie Klasą: {activeClass.className}
                </h4>
                <ul className={styles.navList}>
                  <li>
                    <button
                      className={`${styles.navItem} ${
                        activeMenu === "join-requests"
                          ? styles.navItemActive
                          : ""
                      }`}
                      onClick={() => setActiveMenu("join-requests")}
                    >
                      <span className={styles.navIcon}>🚪</span>
                      <span className={styles.navText}>
                        Prośby o dołączenie
                      </span>
                    </button>
                  </li>
                  <li>
                    <button
                      className={`${styles.navItem} ${
                        activeMenu === "my-students" ? styles.navItemActive : ""
                      }`}
                      onClick={() => setActiveMenu("my-students")}
                    >
                      <span className={styles.navIcon}>🧑‍🎓</span>
                      <span className={styles.navText}>Moi Uczniowie</span>
                    </button>
                  </li>
                  <li>
                    <button
                      className={`${styles.navItem} ${
                        activeMenu === "homework" ? styles.navItemActive : ""
                      }`}
                      onClick={() => setActiveMenu("homework")}
                    >
                      <span className={styles.navIcon}>📝</span>
                      <span className={styles.navText}>Zadania Domowe</span>
                    </button>
                  </li>
                </ul>
              </div>
            )}

            {activeClass && (
              <div className={styles.navSection}>
                <h4 className={styles.navSectionTitle}>Komunikacja</h4>
                <ul className={styles.navList}>
                  <li>
                    <button
                      className={`${styles.navItem} ${
                        activeMenu === "student-messages"
                          ? styles.navItemActive
                          : ""
                      }`}
                      onClick={() => setActiveMenu("student-messages")}
                    >
                      <span className={styles.navIcon}>📧</span>
                      <span className={styles.navText}>Wiadomości Uczniów</span>
                    </button>
                  </li>
                  <li>
                    <button
                      className={`${styles.navItem} ${
                        activeMenu === "parent-messages"
                          ? styles.navItemActive
                          : ""
                      }`}
                      onClick={() => setActiveMenu("parent-messages")}
                    >
                      <span className={styles.navIcon}>✉️</span>
                      <span className={styles.navText}>
                        Wiadomości Rodziców
                      </span>
                    </button>
                  </li>
                </ul>
              </div>
            )}
          </nav>
        </aside>

        <main className={styles.mainContent}>{renderContent()}</main>
      </div>
    </div>
  );
}

export default TeacherDashboard;

interface PlaceholderProps {
  title: string;
  classId: string;
}

const PlaceholderView: React.FC<PlaceholderProps> = ({ title, classId }) => (
  <div className={styles.contentSection}>
    <h2 className={styles.contentTitle}>
      <span className={styles.contentIcon}>🚧</span>
      {title}
    </h2>
    <p className={styles.contentDescription}>
      Ten widok jest w trakcie budowy. Pracujesz w kontekście klasy o ID: **
      {classId}**.
    </p>
  </div>
);
