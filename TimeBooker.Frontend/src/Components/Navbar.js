import React from "react";
import { Link, useHistory } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import { FaSignInAlt, FaSignOutAlt } from "react-icons/fa";
import "./Navbar.css";
import { getCookie, deleteCookie } from "../Utilities/cookieUtils";

const Navbar = ({ showLogin = true }) => {
    const history = useHistory();

    const isAuthenticated = () => {
        const sessionId = getCookie("SessionId");
        const userId = getCookie("UserId");
        return sessionId && userId;
    };

    const handleLogout = async () => {
        try {
            const sessionId = getCookie("SessionId");
            const response = await fetch("https://localhost:7000/auth/logout", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ SessionId: sessionId })
            });

            if (!response.ok) {
                throw new Error("Logout failed");
            }

            deleteCookie("SessionId");
            deleteCookie("UserId");

            history.push("/");
            window.location.reload(true);
        } catch (error) {
            console.error('Error loging out:', error);
        }
    };

    const handleReloadWindow = () => {

        history.push("/");
        window.location.reload(true);
    };

    return (
        <nav className="navbar navbar-dark bg-dark">
            <div className="container">
                <Link to="/" className="navbar-brand" onClick={handleReloadWindow}>TimeBooker</Link>
                <ul className="navbar-nav ml-auto">
                    {isAuthenticated() ? (
                        <li className="nav-item">
                            <button onClick={handleLogout} className="nav-link btn btn-outline-light">
                                <FaSignOutAlt className="align-middle icon" />
                                <span className="align-middle text-white text"> Logout</span>
                            </button>
                        </li>
                    ) : (
                        showLogin && (
                            <li className="nav-item">
                                <Link to="/login" className="nav-link btn btn-outline-light">
                                    <FaSignInAlt className="align-middle icon" />
                                    <span className="align-middle text-white text"> Login</span>
                                </Link>
                            </li>
                        )
                    )}
                </ul>
            </div>
        </nav>
    );
}

export default Navbar;
