import React, { useEffect, useState } from "react";
import NotificationModal from "../../notification/modals/notificationModal.tsx";
import Learning from "./learning.tsx";
import MyBadges from "./myBadges.tsx";
import MyAchievements from "./myAchievements.tsx";
import AllAchievements from "./allAchievements.tsx";
import MyClass from "./myClass.tsx";
import Homework from "./homework.tsx";
import styles from "../styles/studentDashboard.module.css";
import NavBar from "../../../components/NavBar.tsx";
import Challenge from "./challenge.tsx";
import Review from "./review.tsx";
import StudentService, {
  type StudentProgressDetails,
} from "../studentService.tsx";
import Contact from "./Contact.tsx";
import RewardService from "../../classroom/RewardService.tsx";

type MenuItem =
  | "challenge"
  | "learning"
  | "review"
  | "my-class"
  | "homework"
  | "my-badges"
  | "my-achievements"
  | "all-achievements"
  | "contact";

function StudentDashboard() {
  const [activeMenu, setActiveMenu] = useState<MenuItem>("learning");
  const [progress, setProgress] = useState<StudentProgressDetails | null>(null);
  const [showLevelUp, setShowLevelUp] = useState(false);
  const [showNewReward, setShowNewReward] = useState(false);
  const [showNotifications, setShowNotifications] = useState(false);

  const fetchAccountLevelDetails = async () => {
    try {
      const response = await StudentService.GetStudentProgress(
        progress?.level || 0
      );
      setProgress(response);

      if (response.newLevel) {
        setShowLevelUp(true);
        setTimeout(() => setShowLevelUp(false), 3000);

        await RewardService.MarkAsSeen();
      }
    } catch (error) {
      console.error("Błąd podczas pobierania postępu:", error);
    }
  };

  const fetchProgressAndRewards = async () => {
    await fetchAccountLevelDetails();
    await AreAnyNewRewards();
  };

  const AreAnyNewRewards = async () => {
    try {
      const response = await RewardService.AreAnyNewRewards();

      if (response) {
        setShowNewReward(true);
        setTimeout(() => setShowNewReward(false), 3000);
      }
    } catch (error) {
      console.error("Błąd podczas pobierania postępu:", error);
    }
  };

  useEffect(() => {
    fetchAccountLevelDetails();
    AreAnyNewRewards();
  }, []);

  const renderContent = () => {
    switch (activeMenu) {
      case "learning":
        return <Learning onClose={fetchProgressAndRewards} />;
      case "challenge":
        return <Challenge onClose={fetchProgressAndRewards} />;
      case "review":
        return <Review onClose={fetchProgressAndRewards} />;
      case "my-class":
        return <MyClass publicId={null} />;
      case "homework":
        return <Homework />;
      case "my-badges":
        return <MyBadges />;
      case "my-achievements":
        return <MyAchievements />;
      case "all-achievements":
        return <AllAchievements />;
      case "contact":
        return <Contact />;
      default:
        return null;
    }
  };

  const calculateProgress = () => {
    if (!progress) return 0;
    const current = progress.currentPoints - progress.minLevelPoints;
    const max = progress.maxLevelPoints - progress.minLevelPoints;
    return Math.min(Math.max((current / max) * 100, 0), 100);
  };

  return (
    <div className={styles.page}>
      <NavBar />

      <NotificationModal
        isOpen={showNotifications}
        onClose={() => setShowNotifications(false)}
      />

      {showLevelUp && (
        <div className={styles.levelUpPopup}>
          <div className={styles.levelUpContent}>
            <span className={styles.levelUpIcon}>🎉</span>
            <h3 className={styles.levelUpTitle}>Nowy Poziom!</h3>
            <p className={styles.levelUpText}>
              Awansowałeś na poziom {progress?.level}
            </p>
          </div>
        </div>
      )}

      {showNewReward && (
        <div className={styles.levelUpPopup}>
          <div className={styles.levelUpContent}>
            <span className={styles.levelUpIcon}>🎉</span>
            <h3 className={styles.levelUpTitle}>Zdobyto nową nagrodę!</h3>
            <p className={styles.levelUpText}>Sprawdź w zakładce nagród</p>
          </div>
        </div>
      )}

      <div className={styles.dashboardContainer}>
        <aside className={styles.sidebar}>
          <div className={styles.sidebarHeader}>
            <h3 className={styles.sidebarTitle}>Dashboard Ucznia</h3>

            <div className={styles.userIcons}>
              <div className={styles.userBadge}>
                <span className={styles.userIcon}>👨‍🎓</span>
              </div>
              <button
                className={styles.notificationButton}
                onClick={() => setShowNotifications(true)}
                title="Powiadomienia"
              >
                <span className={styles.userIcon}>🔔</span>
              </button>
            </div>

            {progress && (
              <div className={styles.progressSection}>
                <div className={styles.levelInfo}>
                  <span className={styles.levelLabel}>Poziom</span>
                  <span className={styles.levelNumber}>{progress.level}</span>
                </div>

                <div className={styles.expBar}>
                  <div className={styles.expBarLabel}>
                    <span>EXP</span>
                    <span>
                      {progress.currentPoints} / {progress.maxLevelPoints}
                    </span>
                  </div>
                  <div className={styles.expBarTrack}>
                    <div
                      className={styles.expBarFill}
                      style={{ width: `${calculateProgress()}%` }}
                    />
                  </div>
                </div>
              </div>
            )}
          </div>

          <nav className={styles.navigation}>
            <div className={styles.navSection}>
              <h4 className={styles.navSectionTitle}>Ścieżki nauki</h4>
              <ul className={styles.navList}>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "learning" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("learning")}
                  >
                    <span className={styles.navIcon}>📚</span>
                    <span className={styles.navText}>Nauka</span>
                  </button>
                </li>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "challenge" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("challenge")}
                  >
                    <span className={styles.navIcon}>🎮</span>
                    <span className={styles.navText}>Zabawa</span>
                  </button>
                </li>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "review" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("review")}
                  >
                    <span className={styles.navIcon}>🔄</span>
                    <span className={styles.navText}>Powtórka</span>
                  </button>
                </li>
              </ul>
            </div>

            <div className={styles.navSection}>
              <h4 className={styles.navSectionTitle}>Moja klasa</h4>
              <ul className={styles.navList}>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "my-class" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("my-class")}
                  >
                    <span className={styles.navIcon}>👥</span>
                    <span className={styles.navText}>Ranking uczniów</span>
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
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "contact" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("contact")}
                  >
                    <span className={styles.navIcon}>📩</span>
                    <span className={styles.navText}>
                      Kontakt z nauczycielem
                    </span>
                  </button>
                </li>
              </ul>
            </div>

            <div className={styles.navSection}>
              <h4 className={styles.navSectionTitle}>Nagrody</h4>
              <ul className={styles.navList}>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "my-badges" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("my-badges")}
                  >
                    <span className={styles.navIcon}>🏅</span>
                    <span className={styles.navText}>Moje odznaki</span>
                  </button>
                </li>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "my-achievements"
                        ? styles.navItemActive
                        : ""
                    }`}
                    onClick={() => setActiveMenu("my-achievements")}
                  >
                    <span className={styles.navIcon}>🏆</span>
                    <span className={styles.navText}>Moje osiągnięcia</span>
                  </button>
                </li>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "all-achievements"
                        ? styles.navItemActive
                        : ""
                    }`}
                    onClick={() => setActiveMenu("all-achievements")}
                  >
                    <span className={styles.navIcon}>🎯</span>
                    <span className={styles.navText}>
                      Wszystkie osiągnięcia
                    </span>
                  </button>
                </li>
              </ul>
            </div>
          </nav>
        </aside>

        <main className={styles.mainContent}>{renderContent()}</main>
      </div>
    </div>
  );
}

export default StudentDashboard;
