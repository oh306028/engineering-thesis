import React, { useEffect, useState } from "react";
import styles from "../styles/StudentComponents.module.css";
import RewardService, {
  type BadgeDetails,
} from "../../classroom/RewardService.tsx";

function MyBadges() {
  const [badges, setBadges] = useState<BadgeDetails[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchBadges = async () => {
      try {
        const data = await RewardService.GetMineBadges();
        setBadges(data);
        await RewardService.MarkAsSeen();
      } catch (error) {
        console.error("Błąd podczas pobierania odznak:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchBadges();
  }, []);

  if (loading) {
    return (
      <div className={styles.loadingContainer}>
        <div className={styles.spinner}></div>
        <p>Ładowanie odznak...</p>
      </div>
    );
  }

  return (
    <div className={styles.contentSection}>
      <div className={styles.header}>
        <h2 className={styles.title}>
          <span className={styles.icon}>🏅</span>
          Moje Odznaki
        </h2>
        <p className={styles.subtitle}>
          Zdobyte odznaki za ukończone ścieżki nauki
        </p>
      </div>

      {/* Stats Card */}
      <div className={styles.pointsCard}>
        <div className={styles.pointsContent}>
          <span className={styles.pointsIcon}>🏅</span>
          <div>
            <div className={styles.pointsLabel}>Zdobyte odznaki</div>
            <div className={styles.pointsValue}>{badges.length}</div>
          </div>
        </div>
        <div className={styles.levelBadge}>
          <span className={styles.levelIcon}>⭐</span>
          <span>Kolekcja</span>
        </div>
      </div>

      {badges.length === 0 ? (
        <div className={styles.emptyState}>
          <span className={styles.emptyIcon}>🔒</span>
          <p>Nie masz jeszcze żadnych odznak</p>
          <p className={styles.subtitle}>
            Ukończ ścieżki nauki, aby zdobywać odznaki!
          </p>
        </div>
      ) : (
        <div className={styles.achievementsSection}>
          <h3 className={styles.sectionTitle}>Twoja kolekcja odznak</h3>
          <div className={styles.badgesGrid}>
            {badges.map((badge) => (
              <div key={badge.publicId} className={styles.badge}>
                <span className={styles.badgeIcon}>{badge.emote}</span>
                <div className={styles.badgeName}>{badge.name}</div>
                <div className={styles.badgeDescription}>Odznaka zdobyta</div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

export default MyBadges;
