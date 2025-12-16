import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import styles from "./NavBar.module.css";

const NavBar: React.FC = () => {
  const navigate = useNavigate();
  const [scrolled, setScrolled] = useState(false);
  const isAuthenticated = !localStorage.getItem("token");

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 50);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const logOut = () => {
    localStorage.removeItem("token");
    navigate("/");
  };

  return (
    <header className={`${styles.nav} ${scrolled ? styles.navScrolled : ""}`}>
      <div onClick={() => navigate("/")} className={styles.brand}>
        <span className={styles.brandIcon}>🎨</span>
        Play & Learn
      </div>
      {isAuthenticated ? (
        <nav className={styles.links}>
          <button
            className={`${styles.link} ${styles.linkRegister}`}
            onClick={() => navigate("/accounts/register")}
          >
            <span className={styles.linkIcon}></span>
            Dołącz teraz
          </button>
          <button
            className={`${styles.link} ${styles.linkRegister}`}
            onClick={() => navigate("/accounts/login")}
          >
            <span className={styles.linkIcon}></span>
            Zaloguj
          </button>
          <button
            className={`${styles.link} ${styles.linkRegister}`}
            onClick={() => navigate("/accounts/register-teacher")}
          >
            <span className={styles.linkIcon}></span>
            Kącik nauczyciela
          </button>
        </nav>
      ) : (
        <nav className={styles.links}>
          <button
            className={`${styles.link} ${styles.linkRegister}`}
            onClick={() => navigate("/account/profile-details")}
          >
            <span className={styles.linkIcon}></span>
            Mój profil
          </button>

          <button
            className={`${styles.link} ${styles.linkRegister}`}
            onClick={() => logOut()}
          >
            <span className={styles.linkIcon}></span>
            Wyloguj się
          </button>
        </nav>
      )}
    </header>
  );
};

export default NavBar;
