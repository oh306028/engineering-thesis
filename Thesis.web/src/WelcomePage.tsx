import React from "react";
import NavBar from "./components/NavBar";
import FeatureSection from "./components/FeatureSection";
import styles from "./WelcomePage.module.css";
import img1 from "./assets/img1.png";
import img2 from "./assets/img2.png";
import img3 from "./assets/img3.png";
import img4 from "./assets/img4.png";

const WelcomePage: React.FC = () => {
  return (
    <div className={styles.page}>
      <NavBar />

      <main className={styles.content}>
        {/* Hero Section */}
        <section className={styles.hero}>
          <h1 className={styles.title}>
            <span className={styles.titleSparkle}>✨</span>
            Cześć!
            <span className={styles.titleSparkle}>✨</span>
          </h1>
          <p className={styles.lead}>
            Tworzymy platformę, która łączy dobrą <b>zabawę</b>, rywalizację i{" "}
            <b>edukację</b> w jednym miejscu. To tutaj znajdziesz ciekawe
            zadania, a za poprawne odpowiedzi zdobywasz <b>nagrody!</b> Dołącz
            do swojej <b>wirtualnej </b>
            klasy i baw się już dziś!
          </p>

          <div className={styles.heroBadges}>
            <div className={`${styles.badge} ${styles.badge1}`}>
              <span className={styles.badgeIcon}>🎮</span>
              Zdobywaj nagrody za zadania
            </div>
            <div className={`${styles.badge} ${styles.badge2}`}>
              <span className={styles.badgeIcon}>🎯</span>
              Rywalizuj z kolegami i koleżankami
            </div>
            <div className={`${styles.badge} ${styles.badge3}`}>
              <span className={styles.badgeIcon}>📖</span>
              Ćwicz materiał przed kartkówką
            </div>
          </div>
        </section>

        <FeatureSection
          image={img2}
          title="Kolorowy świat nauki"
          description="Nasze gry edukacyjne zostały zaprojektowane z myślą o najmłodszych. Jasne kolory, przyjazne postacie i intuicyjny interfejs sprawiają, że nauka staje się przyjemnością. Dzieci rozwijają umiejętności matematyczne, językowe i logiczne w bezpiecznym, kontrolowanym środowisku."
          reversed={false}
          accent="yellow"
        />

        <FeatureSection
          image={img1}
          title="Rozwój przez zabawę"
          description="Każda gra została stworzona przez ekspertów w dziedzinie edukacji wczesnoszkolnej. Nasza metodyka łączy elementy gamifikacji z programem nauczania, pozwalając dzieciom na naturalny rozwój w tempie dostosowanym do ich potrzeb. Rodzice mogą śledzić postępy swoich pociech."
          reversed={true}
          accent="pink"
        />

        <FeatureSection
          image={img3}
          title="Wsparcie dla nauczycieli"
          description="W kąciku nauczyciela znajdziesz gotowe plany lekcji, karty pracy do wydruku oraz interaktywne prezentacje. Wszystkie materiały są zgodne z podstawą programową i mogą być swobodnie wykorzystywane na zajęciach. Regularnie dodajemy nowe zasoby opracowane przez doświadczonych pedagogów."
          reversed={false}
          accent="green"
        />

        <FeatureSection
          image={img4}
          title="Bezpieczeństwo na pierwszym miejscu"
          description="Bezpieczeństwo dzieci jest dla nas priorytetem. Platforma nie zawiera reklam, zewnętrznych linków ani możliwości kontaktu z obcymi osobami. Rodzice mają pełną kontrolę nad kontem dziecka i mogą w każdej chwili sprawdzić jego aktywność. Wszystkie dane są szyfrowane i chronione zgodnie z najwyższymi standardami."
          reversed={true}
          accent="blue"
        />

        <section className={styles.ctaSection}>
          <div className={styles.ctaBox}>
            <h2 className={styles.ctaTitle}>Gotowy na przygodę z nauką?</h2>
            <p className={styles.ctaText}>
              Dołącz do tysięcy zadowolonych rodzin i nauczycieli, którzy już
              korzystają z EduFun!
            </p>
            <button className={styles.ctaButton}>
              Zacznij za darmo
              <span className={styles.ctaArrow}>→</span>
            </button>
          </div>
        </section>

        <footer className={styles.footer}>
          <div className={styles.footerContent}>
            <div className={styles.footerBrand}>
              <div className={styles.brand}>P&L - Play & Learn</div>
              <p className={styles.footerTagline}>
                Edukacja przez zabawę dla najmłodszych
              </p>
            </div>
            <div className={styles.footerLinks}>
              <a href="#" className={styles.footerLink}>
                O nas
              </a>
              <a href="#" className={styles.footerLink}>
                Kontakt
              </a>
              <a href="#" className={styles.footerLink}>
                Polityka prywatności
              </a>
            </div>
          </div>
          <div className={styles.footerBottom}>© 2025 O.H.</div>
        </footer>
      </main>
    </div>
  );
};

export default WelcomePage;
