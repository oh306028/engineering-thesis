import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom"; // Dodajemy useNavigate
import styles from "./ProfileDetails.module.css";
import AccountService, {
  type ProfileDetails as ProfileDetailsModel,
} from "../modules/account/accountService";

function ProfileDetails() {
  const navigate = useNavigate(); // Inicjalizacja hooka navigate
  const [profile, setProfile] = useState<ProfileDetailsModel | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    try {
      const data = await AccountService.MyProfile();
      setProfile(data);
    } catch (error) {
      console.error("Błąd podczas pobierania profilu:", error);
      setError("Nie udało się załadować danych profilu");
    } finally {
      setLoading(false);
    }
  };

  const handleBack = () => {
    navigate("/dashboard", { replace: true });
  };

  const ProfileContainer = ({ children }: { children: React.ReactNode }) => (
    <div className={styles.container}>
      <div className={styles.formContainer}>{children}</div>
    </div>
  );

  if (loading) {
    return (
      <ProfileContainer>
        <div className={styles.loadingContainer}>
          <div className={styles.spinner}></div>
          <p>Ładowanie profilu...</p>
        </div>
        <button className={styles.backButton} onClick={handleBack}>
          <span className={styles.buttonIcon}>⮜</span> Wróć do strony głównej
        </button>
      </ProfileContainer>
    );
  }

  if (error || !profile) {
    return (
      <ProfileContainer>
        <div className={styles.errorContainer}>
          <span className={styles.errorIcon}>⚠️</span>
          <p>{error || "Błąd ładowania profilu"}</p>
        </div>
        <button className={styles.backButton} onClick={handleBack}>
          <span className={styles.buttonIcon}>⮜</span> Wróć do strony głównej
        </button>
      </ProfileContainer>
    );
  }

  return (
    <div className={styles.container}>
      <div className={styles.formContainer}>
        <h2 className={styles.formTitle}>Mój Profil</h2>
        <p className={styles.formSubtitle}>
          Tutaj znajdziesz szczegółowe informacje o Twoim koncie.
        </p>

        <div className={styles.profileBody}>
          <div className={styles.detailGroup}>
            <div className={styles.detailIcon}>👤</div>
            <div className={styles.detailContent}>
              <div className={styles.detailLabel}>Imię i nazwisko</div>
              <div className={styles.detailValue}>{profile.fullName}</div>
            </div>
          </div>

          <div className={styles.detailGroup}>
            <div className={styles.detailIcon}>📧</div>
            <div className={styles.detailContent}>
              <div className={styles.detailLabel}>Email</div>
              <div className={styles.detailValue}>{profile.email ?? "-"}</div>
            </div>
          </div>

          <div className={styles.detailGroup}>
            <div className={styles.detailIcon}>🔑</div>
            <div className={styles.detailContent}>
              <div className={styles.detailLabel}>Login</div>
              <div className={styles.detailValue}>{profile.login}</div>
            </div>
          </div>
        </div>

        <button className={styles.backButton} onClick={handleBack}>
          <span className={styles.buttonIcon}>⮜</span> Wróć do strony głównej
        </button>
      </div>
    </div>
  );
}

export default ProfileDetails;
