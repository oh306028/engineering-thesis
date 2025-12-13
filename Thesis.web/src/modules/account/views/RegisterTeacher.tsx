import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import NavBar from "../../../components/NavBar.tsx";
import styles from "../styles/Register.module.css";
import AccountService from "../accountService.tsx";
import { toast } from "react-toastify";

interface TeacherFormError {
  [key: string]: string[] | undefined;
}

import { type TeacherAccountRegisterModel } from "../accountService.tsx";

export default function TeacherRegister() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState<TeacherAccountRegisterModel>({
    email: "",
    password: "",
    confirmPassword: "",
    login: "",
    firstName: "",
    lastName: "",
    file: null as any,
  });

  const [errors, setErrors] = useState<TeacherFormError>({});
  const [generalError, setGeneralError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, files } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]: files ? files[0] : value,
    }));

    if (errors[name]) {
      setErrors((prev) => {
        const updated = { ...prev };
        delete updated[name];
        return updated;
      });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrors({});
    setGeneralError("");
    setLoading(true);

    try {
      const send = new FormData();
      Object.entries(formData).forEach(([key, val]) =>
        send.append(key, val as any)
      );

      await AccountService.RegisterTeacher(send);
      toast.success("Poprawnie zarejestrowano! Proszę czekać na akceptację", {
        position: "bottom-center",
        autoClose: 10000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        className: "custom-toast",
        onClose: () => {
          navigate("/");
        },
      });
    } catch (error: any) {
      if (error.response?.status === 422) {
        const valErrors: TeacherFormError = {};
        Object.keys(error.response.data).forEach((key) => {
          valErrors[key] = error.response.data[key];
        });
        setErrors(valErrors);
      } else {
        setGeneralError("Wystąpił błąd podczas rejestracji nauczyciela.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.page}>
      <NavBar />

      <main className={styles.container}>
        <div className={styles.leftSectionTeacher}>
          <div className={styles.textContent}>
            <h1 className={styles.title}>
              <span className={styles.titleIcon}>📘</span>
              Rejestracja nauczyciela
            </h1>

            <p className={styles.description}>
              Wypełnij krótki formularz, aby utworzyć konto nauczyciela i
              rozpocząć pracę z platformą.
            </p>
          </div>
        </div>

        <div className={styles.rightSection}>
          <div className={styles.formContainer}>
            <h2 className={styles.formTitle}>Utwórz konto nauczyciela</h2>

            {generalError && (
              <div className={styles.generalError}>{generalError}</div>
            )}

            <form onSubmit={handleSubmit} className={styles.form}>
              <h3 className={styles.sectionTitle}>Twoje dane</h3>

              <div className={styles.row}>
                <div className={styles.formGroup}>
                  <input
                    name="firstName"
                    placeholder="Imię"
                    value={formData.firstName}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.FirstName ? styles.inputError : ""
                    }`}
                  />
                  {errors.FirstName?.map((e, i) => (
                    <span key={i} className={styles.errorMessage}>
                      {e}
                    </span>
                  ))}
                </div>

                <div className={styles.formGroup}>
                  <input
                    name="lastName"
                    placeholder="Nazwisko"
                    value={formData.lastName}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.LastName ? styles.inputError : ""
                    }`}
                  />
                  {errors.LastName?.map((e, i) => (
                    <span key={i} className={styles.errorMessage}>
                      {e}
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
                    placeholder="Email"
                    value={formData.email}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.Email ? styles.inputError : ""
                    }`}
                  />
                  {errors.Email?.map((e, i) => (
                    <span key={i} className={styles.errorMessage}>
                      {e}
                    </span>
                  ))}
                </div>

                <div className={styles.formGroup}>
                  <input
                    name="login"
                    placeholder="Login"
                    value={formData.login}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.Login ? styles.inputError : ""
                    }`}
                  />
                  {errors.Login?.map((e, i) => (
                    <span key={i} className={styles.errorMessage}>
                      {e}
                    </span>
                  ))}
                </div>
              </div>

              <div className={styles.row}>
                <div className={styles.formGroup}>
                  <input
                    type="password"
                    name="password"
                    placeholder="Hasło"
                    value={formData.password}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.Password ? styles.inputError : ""
                    }`}
                  />
                  {errors.Password?.map((e, i) => (
                    <span key={i} className={styles.errorMessage}>
                      {e}
                    </span>
                  ))}
                </div>

                <div className={styles.formGroup}>
                  <input
                    type="password"
                    name="confirmPassword"
                    placeholder="Powtórz hasło"
                    value={formData.confirmPassword}
                    onChange={handleChange}
                    className={`${styles.input} ${
                      errors.ConfirmPassword ? styles.inputError : ""
                    }`}
                  />
                  {errors.ConfirmPassword?.map((e, i) => (
                    <span key={i} className={styles.errorMessage}>
                      {e}
                    </span>
                  ))}
                </div>
              </div>

              <h3 className={styles.sectionTitle}>Certyfikat nauczyciela</h3>

              <div className={styles.formGroup}>
                <input
                  type="file"
                  name="file"
                  accept="image/*"
                  onChange={handleChange}
                  className={styles.hiddenFileInput}
                  id="fileUpload"
                />
                <label htmlFor="fileUpload" className={styles.fileUploadButton}>
                  📁
                </label>
                {formData.file && (
                  <span className={styles.fileName}>{formData.file.name}</span>
                )}
                {errors.File?.map((e, i) => (
                  <span key={i} className={styles.errorMessage}>
                    {e}
                  </span>
                ))}
              </div>

              <button
                type="submit"
                disabled={loading}
                className={styles.submitButton}
              >
                {loading ? (
                  <span className={styles.spinner}></span>
                ) : (
                  "Zarejestruj nauczyciela →"
                )}
              </button>
            </form>

            <button className={styles.backButton} onClick={() => navigate("/")}>
              ← Powrót do strony głównej
            </button>
          </div>
        </div>
      </main>
    </div>
  );
}
