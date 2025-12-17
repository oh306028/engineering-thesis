import React, { useEffect, useState } from "react";
import styles from "../styles/StudentComponents.module.css";
import RewardService, {
  type AchievementDetails,
} from "../../classroom/RewardService.tsx";

function MyAchievements() {
  const [achievements, setAchievements] = useState<AchievementDetails[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAchievements = async () => {
      try {
        const data = await RewardService.GetMineAchievements();
        setAchievements(data);

        await RewardService.MarkAsSeen();
      } catch (error) {
        console.error("Błąd podczas pobierania osiągnięć:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchAchievements();
  }, []);

  if (loading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie osiągnięć...</p>
      </div>
    );
  }

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>🏆</span>
          Moje Osiągnięcia
        </h2>
        <p className={styles.subtitle}>Zdobyte osiągnięcia i nagrody</p>
      </div>

      {/* Stats Card */}
      <div className={styles.pointsCard}>
        <div className={styles.pointsContent}>
          <span className={styles.pointsIcon}>🏆</span>
          <div>
            <div className={styles.pointsLabel}>Zdobyte osiągnięcia</div>
            <div className={styles.pointsValue}>{achievements.length}</div>
          </div>
        </div>
        <div className={styles.levelBadge}>
          <span className={styles.levelIcon}>🎯</span>
          <span>Rewards</span>
        </div>
      </div>

      {achievements.length === 0 ? (
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>🎯</span>
          <p>Nie masz jeszcze żadnych osiągnięć</p>
          <p className={styles.subtitle}>
            Kontynuuj naukę, aby odblokowywać osiągnięcia!
          </p>
        </div>
      ) : (
        <div className={styles.rewardsSection}>
          <h3 className={styles.sectionTitle}>Lista osiągnięć</h3>
          <div className={styles.recentRewards}>
            {achievements.map((achievement, index) => (
              <div key={index} className={styles.rewardItem}>
                <span className={styles.rewardIcon}>
                  {achievement.badge.emote}
                </span>
                <div className={styles.rewardInfo}>
                  <div className={styles.rewardTitle}>{achievement.name}</div>
                  <div className={styles.rewardDate}>
                    {achievement.description}
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

export default MyAchievements;
