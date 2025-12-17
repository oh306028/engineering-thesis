import React, { useState } from "react";
import NavBar from "../../../components/NavBar.tsx";
import styles from "../styles/adminDashboard.module.css";
import TeacherRequests from "./teacherRequests.tsx";
import SystemUsers from "./systemUsers.tsx";
import SystemClasses from "./systemClasses.tsx";
import AdminMonitoring from "./adminMonitoring.tsx";

type MenuItem =
  | "teacher-requests"
  | "system-users"
  | "system-classes"
  | "monitoring";

function AdminDashboard() {
  const [activeMenu, setActiveMenu] = useState<MenuItem>("teacher-requests");

  const handleAppStatus = () => {
    window.open("http://localhost:3000", "_blank");
  };

  const renderContent = () => {
    switch (activeMenu) {
      case "teacher-requests":
        return <TeacherRequests />;
      case "system-users":
        return <SystemUsers />;
      case "system-classes":
        return <SystemClasses />;
      case "monitoring":
        return <AdminMonitoring />;
      default:
        return null;
    }
  };

  return (
    <div className={styles.page}>
      <NavBar />

      <div className={styles.dashboardContainer}>
        <aside className={styles.sidebar}>
          <div className={styles.sidebarHeader}>
            <h3 className={styles.sidebarTitle}>Dashboard Admina</h3>
            <div className={styles.userBadge}>
              <span className={styles.userIcon}>👨‍💼</span>
            </div>
            <div className={styles.roleLabel}>Panel Administratora</div>
          </div>

          <nav className={styles.navigation}>
            <div className={styles.navSection}>
              <h4 className={styles.navSectionTitle}>Zarządzanie</h4>
              <ul className={styles.navList}>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "teacher-requests"
                        ? styles.navItemActive
                        : ""
                    }`}
                    onClick={() => setActiveMenu("teacher-requests")}
                  >
                    <span className={styles.navIcon}>🙋‍♂️</span>
                    <span className={styles.navText}>Prośby nauczycieli</span>
                  </button>
                </li>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "system-users" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("system-users")}
                  >
                    <span className={styles.navIcon}>👥</span>
                    <span className={styles.navText}>Użytkownicy systemu</span>
                  </button>
                </li>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "system-classes"
                        ? styles.navItemActive
                        : ""
                    }`}
                    onClick={() => setActiveMenu("system-classes")}
                  >
                    <span className={styles.navIcon}>🏫</span>
                    <span className={styles.navText}>Klasy w systemie</span>
                  </button>
                </li>
              </ul>
            </div>

            <div className={styles.navSection}>
              <h4 className={styles.navSectionTitle}>Monitoring</h4>
              <ul className={styles.navList}>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "monitoring" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("monitoring")}
                  >
                    <span className={styles.navIcon}>📊</span>
                    <span className={styles.navText}>Monitoring</span>
                  </button>
                </li>
              </ul>
            </div>

            <div className={styles.navSection}>
              <h4 className={styles.navSectionTitle}>System</h4>
              <ul className={styles.navList}>
                <li>
                  <button className={styles.navItem} onClick={handleAppStatus}>
                    <span className={styles.navIcon}>⚙️</span>
                    <span className={styles.navText}>Stan aplikacji</span>
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

export default AdminDashboard;
