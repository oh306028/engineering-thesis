import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import NavBar from "../../../components/NavBar.tsx";
import styles from "../styles/Login.module.css";
import type { LoginForm, ValidationError } from "../accountService.tsx";
import AccountService from "../accountService.tsx";

function Login() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<LoginForm>({
    login: "",
    password: "",
  });
  const [errors, setErrors] = useState<ValidationError>({});
  const [isLoading, setIsLoading] = useState(false);
  const [generalError, setGeneralError] = useState<string>("");

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));

    if (errors[name]) {
      setErrors((prev) => {
        const newErrors = { ...prev };
        delete newErrors[name];
        return newErrors;
      });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setErrors({});
    setGeneralError("");

    try {
      const response = await AccountService.Login(formData);
      if (response) {
        localStorage.setItem("token", response);
        navigate("/dashboard");
      }
    } catch (error: any) {
      if (error.response?.status === 422) {
        const validationErrors: ValidationError = {};
        if (error.response.data) {
          Object.keys(error.response.data).forEach((key) => {
            validationErrors[key] = error.response.data[key];
          });
        }

        setErrors(validationErrors);
      } else if (error.response?.status === 404) {
        setGeneralError(error.response.data.error);
      } else {
        setGeneralError("Wystąpił błąd. Spróbuj ponownie później.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles.page}>
      <NavBar />

      <main className={styles.container}>
        <div className={styles.leftSection}>
          <div className={styles.textContent}>
            <h1 className={styles.title}>
              <span className={styles.titleIcon}>👋</span>
              Hej!
            </h1>
            <p className={styles.description}>
              Aby korzystać z platformy i móc bawić się dalej, musisz się
              zalogować na swoje <b>konto!</b> <br />
              Pamiętaj, że możesz to zrobić z pomocą rodzica. <br />
              <b>Rodzicu!</b> Po zalogowaniu na swoje konto, możesz przełączyć
              się na konto <b>ucznia!</b>
            </p>

            <div className={styles.features}>
              <div className={styles.feature}>
                <span className={styles.featureIcon}>🏆</span>
                <span>Rozwiązuj zadania i zdobywaj nagrody!</span>
              </div>
              <div className={styles.feature}>
                <span className={styles.featureIcon}>🎮</span>
                <span>Odkrywaj kolejne poziomy</span>
              </div>
              <div className={styles.feature}>
                <span className={styles.featureIcon}>✨</span>
                <span>Baw się dobrze!</span>
              </div>
            </div>
          </div>
        </div>

        <div className={styles.rightSection}>
          <div className={styles.formContainer}>
            <h2 className={styles.formTitle}>Zaloguj się</h2>
            <p className={styles.formSubtitle}>
              Nie masz konta?{" "}
              <button
                onClick={() => navigate("/accounts/register")}
                className={styles.linkButton}
              >
                Dołącz już teraz!
              </button>
            </p>

            {generalError && (
              <div className={styles.generalError}>{generalError}</div>
            )}

            <form onSubmit={handleSubmit} className={styles.form}>
              <div className={styles.formGroup}>
                <label htmlFor="login" className={styles.label}>
                  Login
                </label>
                <input
                  type="text"
                  id="login"
                  name="login"
                  value={formData.login}
                  onChange={handleChange}
                  className={`${styles.input} ${
                    errors.Login ? styles.inputError : ""
                  }`}
                  placeholder="Wpisz swój login"
                />
                {errors.Login &&
                  errors.Login.map((error, index) => (
                    <span key={index} className={styles.errorMessage}>
                      {error}
                    </span>
                  ))}
              </div>

              <div className={styles.formGroup}>
                <label htmlFor="password" className={styles.label}>
                  Hasło
                </label>
                <input
                  type="password"
                  id="password"
                  name="password"
                  value={formData.password}
                  onChange={handleChange}
                  className={`${styles.input} ${
                    errors.Password ? styles.inputError : ""
                  }`}
                  placeholder="Wpisz swoje hasło"
                />
                {errors.Password &&
                  errors.Password.map((error, index) => (
                    <span key={index} className={styles.errorMessage}>
                      {error}
                    </span>
                  ))}
              </div>

              <button
                type="submit"
                disabled={isLoading}
                className={styles.submitButton}
              >
                {isLoading ? (
                  <span className={styles.spinner}></span>
                ) : (
                  <>
                    Zaloguj się
                    <span className={styles.buttonIcon}>→</span>
                  </>
                )}
              </button>
            </form>

            <button onClick={() => navigate("/")} className={styles.backButton}>
              ← Powrót do strony głównej
            </button>
          </div>
        </div>
      </main>
    </div>
  );
}

export default Login;
