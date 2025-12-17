import React, { useEffect, useState } from "react";
import MyStudent from "./myStudent.tsx";
import styles from "../styles/parentDashboard.module.css";
import NavBar from "../../../components/NavBar.tsx";
import ClassroomService, {
  type StudentDetailsWithClassroom,
} from "../../classroom/ClassroomService.tsx";
import ParentContact from "./parentContact.tsx";
import Monitoring from "./monitoring.tsx";
import AccountService from "../../account/accountService.tsx";
import { useNavigate } from "react-router-dom";

type MenuItem = "my-student" | "contact-teacher" | "monitoring";

function ParentDashboard() {
  const [activeMenu, setActiveMenu] = useState<MenuItem>("my-student");
  const [studentData, setStudentData] = useState<StudentDetailsWithClassroom>();
  const navigate = useNavigate();

  const renderContent = () => {
    switch (activeMenu) {
      case "my-student":
        return <MyStudent student={studentData} />;
      case "contact-teacher":
        return <ParentContact />;
      case "monitoring":
        return <Monitoring />;
      default:
        return null;
    }
  };

  const fetchStudentWithClassroomData = async () => {
    try {
      const data = await ClassroomService.GetStudentInfoForParent();
      setStudentData(data);
    } catch (ex: any) {
      console.log(ex);
    }
  };

  const logout = async () => {
    try {
      const response = await AccountService.ImpersonateAsStudent();
      if (response) {
        localStorage.setItem("token", response);
        navigate("/dashboard");
      }
    } catch (ex: any) {
      console.log(ex);
    }
  };
  useEffect(() => {
    fetchStudentWithClassroomData();
  }, []);

  return (
    <div className={styles.page}>
      <NavBar />

      <div className={styles.dashboardContainer}>
        <aside className={styles.sidebar}>
          <div className={styles.sidebarHeader}>
            <h3 className={styles.sidebarTitle}>Dashboard Rodzica</h3>
            <div className={styles.userBadge}>
              <span className={styles.userIcon}>👨‍👩‍👧</span>
            </div>
            <div className={styles.roleLabel}>Panel Opiekuna</div>
          </div>

          <nav className={styles.navigation}>
            <div className={styles.navSection}>
              <h4 className={styles.navSectionTitle}>Główne</h4>
              <ul className={styles.navList}>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "my-student" ? styles.navItemActive : ""
                    }`}
                    onClick={() => setActiveMenu("my-student")}
                  >
                    <span className={styles.navIcon}>👨‍🎓</span>
                    <span className={styles.navText}>Mój uczeń</span>
                  </button>
                </li>
                <li>
                  <button
                    className={`${styles.navItem} ${
                      activeMenu === "contact-teacher"
                        ? styles.navItemActive
                        : ""
                    }`}
                    onClick={() => setActiveMenu("contact-teacher")}
                  >
                    <span className={styles.navIcon}>📩</span>
                    <span className={styles.navText}>
                      Kontakt z nauczycielem
                    </span>
                  </button>
                </li>
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
              <h4 className={styles.navSectionTitle}>Zmiana konta</h4>
              <ul className={styles.navList}>
                <li>
                  <button className={styles.navItem} onClick={() => logout()}>
                    <span className={styles.navText}>
                      Przeloguj na konto ucznia
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

export default ParentDashboard;
