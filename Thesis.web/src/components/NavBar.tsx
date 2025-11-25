import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import styles from "./NavBar.module.css";

const NavBar: React.FC = () => {
  const navigate = useNavigate();
  const [scrolled, setScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 50);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  return (
    <header className={`${styles.nav} ${scrolled ? styles.navScrolled : ""}`}>
      <div className={styles.brand}>
        <span className={styles.brandIcon}>🎨</span>
        Play & Learn
      </div>
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
          onClick={() => navigate("/teacher")}
        >
          <span className={styles.linkIcon}></span>
          Kącik nauczyciela
        </button>
      </nav>
    </header>
  );
};

export default NavBar;
