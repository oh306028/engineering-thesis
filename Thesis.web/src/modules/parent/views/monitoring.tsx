import React, { useEffect, useState } from "react";
import styles from "../styles/monitoring.module.css";
import AdminService, {
  type LogginHistoryModel,
} from "../../admin/adminService";

export default function Monitoring() {
  const [logs, setLogs] = useState<LogginHistoryModel[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchLogins = async () => {
    try {
      const result = await AdminService.GetLogginsForStudent();
      setLogs(result);
    } catch (ex) {
      console.error(ex);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchLogins();
  }, []);

  return (
    <div className={styles.container}>
      <div className={styles.titleHeader}>
        <span className={styles.titleIcon}>📊</span>
        <h2 className={styles.titleText}>Historia logowań</h2>
      </div>

      <p className={styles.description}>
        Poniżej znajduje się zarejestrowana aktywność logowań ucznia, wraz z
        informacją o powodzeniu próby.
      </p>

      {loading ? (
        <div className={styles.loadingBox}>⏳ Wczytywanie danych...</div>
      ) : logs.length === 0 ? (
        <div className={styles.emptyBox}>Brak zarejestrowanych logowań.</div>
      ) : (
        <div className={styles.logList}>
          {logs.map((log, index) => (
            <div className={styles.logItem} key={index}>
              <div className={styles.iconBox}>
                {log.isSucceeded ? (
                  <span className={styles.success}>✅</span>
                ) : (
                  <span className={styles.fail}>❌</span>
                )}
              </div>

              <div className={styles.logInfo}>
                <p className={styles.logDate}>
                  {new Date(log.loginDate).toLocaleString("pl-PL")}
                </p>
                <p className={styles.logEmail}>{log.userEmail}</p>
              </div>

              <div
                className={`${styles.status} ${
                  log.isSucceeded ? styles.statusOk : styles.statusFail
                }`}
              >
                {log.isSucceeded ? "Udane logowanie" : "Nieudane logowanie"}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
