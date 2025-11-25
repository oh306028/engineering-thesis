import React from "react";
import styles from "./FeatureSection.module.css";

interface FeatureSectionProps {
  image: string;
  imageAlt?: string;
  title: string;
  description: string;
  reversed?: boolean;
  accent: "yellow" | "pink" | "green" | "blue";
}

const FeatureSection: React.FC<FeatureSectionProps> = ({
  image,
  imageAlt = "",
  title,
  description,
  reversed = false,
  accent,
}) => {
  const isEmoji = image.length <= 2;

  return (
    <section
      className={`${styles.featureSection} ${reversed ? styles.reversed : ""}`}
    >
      <div className={styles.featureContent}>
        <div className={styles.featureImageContainer}>
          <div
            className={`${styles.featureImage} ${
              styles[
                `accent${accent.charAt(0).toUpperCase() + accent.slice(1)}`
              ]
            }`}
          >
            {isEmoji ? (
              <div className={styles.imagePlaceholder}>{image}</div>
            ) : (
              <img
                src={image}
                alt={imageAlt || title}
                className={styles.actualImage}
              />
            )}
          </div>
        </div>
        <div className={styles.featureText}>
          <h2 className={styles.featureTitle}>{title}</h2>
          <p className={styles.featureDescription}>{description}</p>
        </div>
      </div>
    </section>
  );
};

export default FeatureSection;
