import React, { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { MdEmail } from "react-icons/md";
import { FaLock } from "react-icons/fa";
import { Link } from "react-router-dom";
import Navbar from "./Navbar";
import { useHistory } from "react-router-dom";
import { deleteCookie, setCookie, getCookie } from "../Utilities/cookieUtils";


const LoginForm = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [hasCookies, setHasCookies] = useState(false);
    const history = useHistory();

    useEffect(() => {
        checkCookies();
        const redirectIfHasCookies = () => {
            if(hasCookies)
                history.push("/");
        };
        redirectIfHasCookies();
    }, [hasCookies, history]);

    const checkCookies = () => {
        const userId = getCookie("UserId");
        const sessionId = getCookie("SessionId");
        setHasCookies(userId && sessionId);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        try {
            const response = await fetch("https://localhost:7000/auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email,
                    password
                })
            });

            if (!response.ok) {
                setError("Network error");
                return;
            }

            const serverResult = await response.json();

            if (serverResult.success) {
                console.log(serverResult);
                deleteCookie("SessionId");
                deleteCookie("UserId");
                const expirationDate = new Date(Date.now() + 10 * 60 * 1000);
                setCookie("SessionId", serverResult.data.sessionId, expirationDate);
                setCookie("UserId", serverResult.data.userId, expirationDate);
                history.push("/");
            } else {
                setError(serverResult.message);
            }
        } catch (error) {
            setError("Technical error");
        }
    };

    return (
        <div>
            <Navbar showLogin={false}/>
            <div className="container mt-5">
                <div className="row justify-content-center">
                    <div className="col-md-6">
                        <form onSubmit={handleSubmit}>
                            <h1 className="mb-4">Login</h1>
                            {error && <div className="alert alert-danger mb-3" role="alert">{error}</div>}
                            <div className="input-group mb-3">
                                <span className="input-group-text bg-dark text-white"><MdEmail /></span>
                                <input
                                    type="email"
                                    className="form-control"
                                    placeholder="Email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="input-group mb-3">
                                <span className="input-group-text bg-dark text-white"><FaLock /></span>
                                <input
                                    type="password"
                                    className="form-control"
                                    placeholder="Password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                />
                            </div>
                            <button type="submit" className="btn btn-dark text-white w-100 mb-3">Login</button>
                            <div className="text-center">
                                <p>Don't have an account? <Link to="/register" className="text-black">Register</Link></p>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default LoginForm;
