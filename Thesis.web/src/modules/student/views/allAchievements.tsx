import React, { useEffect, useState } from "react";
import styles from "../styles/StudentComponents.module.css";
import RewardService, {
  type AchievementDetails,
} from "../../classroom/RewardService.tsx";

function AllAchievements() {
  const [allAchievements, setAllAchievements] = useState<AchievementDetails[]>(
    []
  );
  const [myAchievements, setMyAchievements] = useState<AchievementDetails[]>(
    []
  );
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [all, mine] = await Promise.all([
          RewardService.GetAchievements(),
          RewardService.GetMineAchievements(),
        ]);
        setAllAchievements(all);
        setMyAchievements(mine);
      } catch (error) {
        console.error("Błąd podczas pobierania osiągnięć:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const isUnlocked = (achievement: AchievementDetails) => {
    return myAchievements.some((my) => my.name === achievement.name);
  };

  if (loading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie osiągnięć...</p>
      </div>
    );
  }

  const unlockedCount = allAchievements.filter(isUnlocked).length;

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>🎯</span>
          Wszystkie Osiągnięcia
        </h2>
        <p className={styles.subtitle}>
          Odkryj i zdobądź wszystkie osiągnięcia!
        </p>
      </div>

      <div className={styles.pointsCard}>
        <div className={styles.pointsContent}>
          <span className={styles.pointsIcon}>🚀</span>
          <div>
            <div className={styles.pointsLabel}>Twój postęp</div>
            <div className={styles.pointsValue}>
              {unlockedCount} / {allAchievements.length}
            </div>
          </div>
        </div>
        <div className={styles.levelBadge}>
          <span className={styles.levelIcon}>
            {unlockedCount === allAchievements.length ? "🎉" : "📊"}
          </span>
          <span>
            {Math.round((unlockedCount / allAchievements.length) * 100)}%
          </span>
        </div>
      </div>

      <div className={styles.achievementsSection}>
        <h3 className={styles.sectionTitle}>Lista osiągnięć</h3>
        <div className={styles.badgesGrid}>
          {allAchievements.map((achievement, index) => {
            const unlocked = isUnlocked(achievement);
            return (
              <div
                key={index}
                className={`${styles.badge} ${
                  unlocked ? "" : styles.badgeLocked
                }`}
              >
                <span className={styles.badgeIcon}>
                  {unlocked ? achievement.badge.emote : "🔒"}
                </span>
                <div className={styles.badgeName}>{achievement.name}</div>
                <div className={styles.badgeDescription}>
                  {unlocked ? achievement.description : "???"}
                </div>
                {!unlocked && (
                  <div
                    className={styles.badgeDescription}
                    style={{
                      fontSize: "0.75rem",
                      marginTop: "8px",
                      fontStyle: "italic",
                    }}
                  >
                    Zdobądź to osiągnięcie
                  </div>
                )}
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
}

export default AllAchievements;
