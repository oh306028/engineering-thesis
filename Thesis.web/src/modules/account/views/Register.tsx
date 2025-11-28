import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import NavBar from "../../../components/NavBar.tsx";
import styles from "../styles/Register.module.css";
import type { RegisterForm, ValidationError } from "../accountService.tsx";
import image from "../../../assets/img1.png";
import AccountService from "../accountService.tsx";

function Register() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState<RegisterForm>({
    login: "",
    password: "",
    parentFirstName: "",
    parentLastName: "",
    email: "",
    confirmPassword: "",
    studentFirstName: "",
    studentLastName: "",
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
      const response = await AccountService.Register(formData);

      const loginResponse = await AccountService.Login({
        login: response.login,
        password: response.password,
      });

      localStorage.setItem("token", loginResponse);
      navigate("/dashboard");
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
              Witaj na pokładzie!
            </h1>
            <p className={styles.description}>
              Zachęcamy do utworzenia konta, aby rozpocząć przygodę z naszą
              platformą edukacyjną pełną zabawy i nauki.
            </p>

            <div className={styles.featureContent}>
              <div className={styles.featureImageContainer}>
                <div
                  className={`${styles.featureImage} ${
                    styles[
                      `accent${
                        "green".charAt(0).toUpperCase() + "green".slice(1)
                      }`
                    ]
                  }`}
                >
                  <img src={image} className={styles.actualImage} />
                </div>
              </div>
            </div>

            <p className={styles.description}>
              <b>Uczniu!</b> Pamiętaj aby poprosić rodzica o rejestrację konta.
            </p>
            <p className={styles.description}>
              <b>Rodzicu!</b> Utworzenie konta w systemie jest bezpłatne.
            </p>
          </div>
        </div>

        <div className={styles.rightSection}>
          <div className={styles.formContainer}>
            <h2 className={styles.formTitle}>Zarejestruj się</h2>
            <p className={styles.formSubtitle}>
              Masz już konto?{" "}
              <button
                type="button"
                className={styles.linkButton}
                onClick={() => navigate("/accounts/login")}
              >
                Zaloguj się
              </button>
            </p>

            {generalError && (
              <div className={styles.generalError}>{generalError}</div>
            )}

            <form onSubmit={handleSubmit} className={styles.form}>
              <h3 className={styles.sectionTitle}>Informacje rodzica</h3>

              <div className={styles.row}>
                <div className={styles.formGroup}>
                  <input
                    type="text"
                    name="parentFirstName"
                    value={formData.parentFirstName}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.ParentFirstName ? styles.inputError : ""
                    }`}
                    placeholder="Imię rodzica"
                  />
                  {errors.ParentFirstName &&
                    errors.ParentFirstName.map((error, index) => (
                      <span key={index} className={styles.errorMessage}>
                        {error}
                      </span>
                    ))}
                </div>

                <div className={styles.formGroup}>
                  <input
                    type="text"
                    name="parentLastName"
                    value={formData.parentLastName}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.ParentLastName ? styles.inputError : ""
                    }`}
                    placeholder="Nazwisko rodzica"
                  />
                  {errors.ParentLastName &&
                    errors.ParentLastName.map((error, index) => (
                      <span key={index} className={styles.errorMessage}>
                        {error}
                      </span>
                    ))}
                </div>
              </div>

              <h3 className={styles.sectionTitle}>Informacje ucznia</h3>

              <div className={styles.row}>
                <div className={styles.formGroup}>
                  <input
                    type="text"
                    name="studentFirstName"
                    value={formData.studentFirstName}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.StudentFirstName ? styles.inputError : ""
                    }`}
                    placeholder="Imię ucznia"
                  />
                  {errors.StudentFirstName &&
                    errors.StudentFirstName.map((error, index) => (
                      <span key={index} className={styles.errorMessage}>
                        {error}
                      </span>
                    ))}
                </div>

                <div className={styles.formGroup}>
                  <input
                    type="text"
                    name="studentLastName"
                    value={formData.studentLastName}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.StudentLastName ? styles.inputError : ""
                    }`}
                    placeholder="Nazwisko ucznia"
                  />
                  {errors.StudentLastName &&
                    errors.StudentLastName.map((error, index) => (
                      <span key={index} className={styles.errorMessage}>
                        {error}
                      </span>
                    ))}
                </div>
              </div>

              <h3 className={styles.sectionTitle}>Dane logowania</h3>

              <div className={styles.row}>
                <div className={styles.formGroup}>
                  <input
                    type="text"
                    name="email"
                    value={formData.email}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.Email ? styles.inputError : ""
                    }`}
                    placeholder="Email"
                  />
                  {errors.Email &&
                    errors.Email.map((error, index) => (
                      <span key={index} className={styles.errorMessage}>
                        {error}
                      </span>
                    ))}
                </div>

                <div className={styles.formGroup}>
                  <input
                    type="text"
                    name="login"
                    value={formData.login}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.Login ? styles.inputError : ""
                    }`}
                    placeholder="Login"
                  />
                  {errors.Login &&
                    errors.Login.map((error, index) => (
                      <span key={index} className={styles.errorMessage}>
                        {error}
                      </span>
                    ))}
                </div>
              </div>

              <div className={styles.row}>
                <div className={styles.formGroup}>
                  <input
                    type="password"
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.Password ? styles.inputError : ""
                    }`}
                    placeholder="Hasło"
                  />
                  {errors.Password &&
                    errors.Password.map((error, index) => (
                      <span key={index} className={styles.errorMessage}>
                        {error}
                      </span>
                    ))}
                </div>

                <div className={styles.formGroup}>
                  <input
                    type="password"
                    name="confirmPassword"
                    value={formData.confirmPassword}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.ConfirmPassword ? styles.inputError : ""
                    }`}
                    placeholder="Powtórz hasło"
                  />
                  {errors.ConfirmPassword &&
                    errors.ConfirmPassword.map((error, index) => (
                      <span key={index} className={styles.errorMessage}>
                        {error}
                      </span>
                    ))}
                </div>
              </div>

              <button
                type="submit"
                disabled={isLoading}
                className={styles.submitButton}
              >
                {isLoading ? (
                  <span className={styles.spinner}></span>
                ) : (
                  "Zarejestruj się →"
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

export default Register;
