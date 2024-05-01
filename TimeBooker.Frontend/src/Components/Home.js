import React, { useState, useEffect } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from "./Navbar";
import { getCookie } from "../Utilities/cookieUtils";
import "./Home.css";

const Home = () => {
    const [selectedDate, setSelectedDate] = useState("");
    const [selectedStatus, setSelectedStatus] = useState("");
    const [statuses, setStatuses] = useState([]);
    const [slots, setSlots] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showReserveModal, setshowReserveModal] = useState(false);
    const [email, setEmail] = useState("");
    const [selectedSlotId, setSelectedSlotId] = useState(null);
    const [hasCookies, setHasCookies] = useState(false);
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [startDateTime, setStartDateTime] = useState("");
    const [isRepeating, setIsRepeating] = useState(false);
    const [repeatIntervalInMinutes, setRepeatIntervalInMinutes] = useState("");
    const [endDateTime, setEndDateTime] = useState("");
    const [showEditModal, setShowEditModal] = useState(false);
    const [editedSlot, setEditedSlot] = useState(null);

    useEffect(() => {
        fetchSlotStatuses();
        checkCookies();
    }, []);

    useEffect(() => {
        if (!showReserveModal) {
            setEmail("");
        }
    }, [showReserveModal]);

    useEffect(() => {
        if (!showCreateModal) {
            setStartDateTime("");
            setIsRepeating(false);
            setRepeatIntervalInMinutes("");
            setEndDateTime("");
        }
    }, [showCreateModal]);

    useEffect(() => {
        if (!showEditModal) {
            setEditedSlot(null);
        }
    }, [showEditModal]);

    const checkCookies = () => {
        const userId = getCookie("UserId");
        const sessionId = getCookie("SessionId");
        setHasCookies(userId && sessionId);
    };

    const verifyCookies = (callback) => {
        const userId = getCookie("UserId");
        const sessionId = getCookie("SessionId");
        const hasCookies = userId && sessionId;
        if (!hasCookies) {
            reloadHomePage();
            return;
        }
        callback();
    };

    const reloadHomePage = () => {
        window.location.reload(true);
    }

    const fetchSlotStatuses = async () => {
        try {
            const response = await fetch("https://localhost:7000/slots/getSlotStatuses");
            const data = await response.json();
            setStatuses(data.data);
            setLoading(false);
        } catch (error) {
            console.error("Error fetching slot statuses:", error);
            setLoading(false);
        }
    };

    const openEditModal = (slot) => {
        verifyCookies(() => {
            setEditedSlot(slot);
            setShowEditModal(true);
        });
    };

    const openCreateModal = () => {
        verifyCookies(() => {
            setShowCreateModal(true);
        });
    };

    const handleDateChange = (event) => {
        setSelectedDate(event.target.value);
    };

    const handleStatusChange = (event) => {
        setSelectedStatus(event.target.value);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            let queryString = "https://localhost:7000/slots/getList";

            if (selectedDate) {
                queryString += `?date=${selectedDate}`;
            }
            if (selectedStatus) {
                queryString += selectedDate ? `&status=${selectedStatus}` : `?status=${selectedStatus}`;
            }

            const response = await fetch(queryString);
            if (response.ok) {
                const data = await response.json();
                const sortedSlots = data.data.sort((a, b) => new Date(a.dateTime) - new Date(b.dateTime));
                setSlots(sortedSlots);
            } else {
                console.error("Failed to fetch slots:", response.statusText);
            }
        } catch (error) {
            console.error("Error fetching slots:", error);
        }
    };

    const getStatusName = (statusValue) => {
        const status = statuses.find(status => status.value === statusValue);
        return status ? status.name : 'Unknown';
    };

    const getStatusClass = (statusValue) => {
        switch (statusValue) {
            case "Available":
                return "available";
            case "Reserved":
                return "reserved";
            case "Booked":
                return "booked";
            default:
                return "";
        }
    };

    const reserveSlot = (slotId) => {
        setSelectedSlotId(slotId);
        setshowReserveModal(true);
    };

    const handleModalSubmit = async (event) => {
        event.preventDefault();
        try {
            const response = await fetch("https://localhost:7000/slots/reserve", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ slotId: selectedSlotId, email }),
            });
            if (response.ok) {
                const responseData = await response.json();
                if (responseData.success) {
                    setshowReserveModal(false);
                    handleSubmit(event);
                } else {
                    console.error("Failed to reserve slot:", responseData.message || response.statusText);
                }
            } else {
                console.error("Failed to reserve slot:", response.statusText);
            }
        } catch (error) {
            console.error("Error reserving slot:", error);
        }
    };

    const handleCreateSlotsSubmit = async (event) => {
        checkCookies();
        if (!hasCookies) {
            reloadHomePage();
            return;
        }
        event.preventDefault();
        const userId = getCookie("UserId");
        const sessionId = getCookie("SessionId");
        const requestData = {
            userId,
            sessionId,
            startDateTime,
            isRepeating,
            repeatIntervalInMinutes: isRepeating ? repeatIntervalInMinutes : null,
            endDateTime: isRepeating ? endDateTime : null,
        };
        try {
            const response = await fetch("https://localhost:7000/slots/create", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(requestData),
            });
            if (response.ok) {
                const responseData = await response.json();
                if (responseData.success) {
                    setShowCreateModal(false);
                    handleSubmit(event);
                } else {
                    console.error("Failed to create slots:", responseData.message || response.statusText);
                }
            } else {
                console.error("Failed to create slots:", response.statusText);
            }
        } catch (error) {
            console.error("Error creating slots:", error);
        }
    };

    const handleDeleteOpen = async (event, slotId) => {
        verifyCookies(() => {
            handleDelete(event, slotId);
        });
    }

    const handleDelete = async (event, slotId) => {
        const userId = getCookie("UserId");
        const sessionId = getCookie("SessionId");
        if (!userId && !sessionId) {
            reloadHomePage();
            return;
        }
        const confirmed = window.confirm("Are you sure you want to delete this slot?");
        if (!confirmed) return;

        try {
            const userId = getCookie("UserId");
            const sessionId = getCookie("SessionId");

            const url = `https://localhost:7000/slots/delete?slotId=${slotId}&sessionId=${sessionId}&userId=${userId}`;

            const response = await fetch(url, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                },
            });
            if (response.ok) {
                const responseData = await response.json();
                if (responseData.success) {
                    setSlots(prevSlots => prevSlots.filter(slot => slot.id !== slotId));
                    handleSubmit(event);
                } else {
                    console.error("Failed to delete slot:", responseData.message || response.statusText);
                }
            } else {
                console.error("Failed to delete slot:", response.statusText);
            }
        } catch (error) {
            console.error("Error deleting slot:", error);
        }
    };

    const handleEditSubmit = async (event) => {
        checkCookies();
        if (!hasCookies) {
            reloadHomePage();
            return;
        }
        event.preventDefault();
        try {
            const response = await fetch("https://localhost:7000/slots/update", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    userId: getCookie("UserId"),
                    sessionId: getCookie("SessionId"),
                    id: editedSlot.id,
                    dateTime: editedSlot.dateTime,
                    email: editedSlot.email,
                    status: parseInt(editedSlot.status),
                }),
            });
            if (response.ok) {
                const responseData = await response.json();
                if (responseData.success) {
                    setShowEditModal(false);
                    handleSubmit(event);
                } else {
                    console.error("Failed to update slot:", responseData.message || response.statusText);
                }
            } else {
                console.error("Failed to update slot:", response.statusText);
            }
        } catch (error) {
            console.error("Error updating slot:", error);
        }
    };

    return (
        <div>
            <Navbar />
            <div className="container mt-4">
                {hasCookies && (
                    <button type="button" className="btn btn-primary mb-3" onClick={() => openCreateModal()}>
                        Create slots
                    </button>
                )}
                <form onSubmit={handleSubmit} className="row g-3 align-items-end">
                    <div className="col">
                        <label htmlFor="inputDate" className="form-label">Date:</label>
                        <input
                            type="date"
                            className="form-control"
                            id="inputDate"
                            value={selectedDate}
                            onChange={handleDateChange}
                        />
                    </div>
                    <div className="col">
                        <label htmlFor="inputStatus" className="form-label">Status:</label>
                        <select
                            className="form-control"
                            id="inputStatus"
                            value={selectedStatus}
                            onChange={handleStatusChange}
                        >
                            <option value="">Select Status</option>
                            {loading ? (
                                <option key="loading" disabled>Loading...</option>
                            ) : (
                                statuses.map((status) => (
                                    <option key={status.value} value={status.value}>
                                        {status.name}
                                    </option>
                                ))
                            )}
                        </select>
                    </div>
                    <div className="col-auto">
                        <button type="submit" className="btn btn-primary">Search</button>
                    </div>
                </form>
                <div className="mt-4">
                    <ul className="list-group">
                        {slots.map((slot) => (
                            <li
                                key={slot.id}
                                className={`list-group-item ${getStatusClass(getStatusName(slot.status))}`}
                            >
                                <div className="d-flex justify-content-between align-items-center">
                                    <div>Date: {new Date(new Date(slot.dateTime).getTime() + (3 * 60 * 60 * 1000)).toISOString().replace('T', ' ').slice(0, 16)}</div>
                                    {hasCookies && (
                                        <div>
                                            <button className="btn btn-danger me-2" onClick={(e) => handleDeleteOpen(e, slot.id)}>Delete</button>
                                            <button className="btn btn-warning" onClick={() => openEditModal(slot)}>Edit</button>
                                        </div>
                                    )}
                                    {!hasCookies && slot.status === 1 && (
                                        <button
                                            className="btn btn-primary"
                                            onClick={() => reserveSlot(slot.id)}
                                        >
                                            Reserve
                                        </button>
                                    )}
                                </div>
                            </li>
                        ))}
                    </ul>
                </div>
            </div>

            <div className="modal" tabIndex="-1" role="dialog" style={{ display: showReserveModal ? "block" : "none" }}>
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <form onSubmit={handleModalSubmit}>
                            <div className="modal-header">
                                <h5 className="modal-title">Enter Your Email</h5>
                            </div>
                            <div className="modal-body">
                                <input type="email" className="form-control" value={email} onChange={(e) => setEmail(e.target.value)} required />
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={() => setshowReserveModal(false)}>Close</button>
                                <button type="submit" className="btn btn-primary">Submit</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            <div className="modal" tabIndex="-1" role="dialog" style={{ display: showCreateModal ? "block" : "none" }}>
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <form onSubmit={handleCreateSlotsSubmit}>
                            <div className="modal-header">
                                <h5 className="modal-title">Slot creation</h5>
                            </div>
                            <div className="modal-body">
                                <label htmlFor="inputStartDateTime" className="form-label">Start Date Time:</label>
                                <input
                                    type="datetime-local"
                                    className="form-control"
                                    id="inputStartDateTime"
                                    value={startDateTime}
                                    onChange={(e) => setStartDateTime(e.target.value)}
                                    required
                                />
                                <div className="form-check mt-2">
                                    <input
                                        className="form-check-input"
                                        type="checkbox"
                                        id="checkIsRepeating"
                                        checked={isRepeating}
                                        onChange={(e) => setIsRepeating(e.target.checked)}
                                    />
                                    <label className="form-check-label" htmlFor="checkIsRepeating">
                                        Is Repeating
                                    </label>
                                </div>
                                {isRepeating && (
                                    <>
                                        <label htmlFor="inputRepeatInterval" className="form-label">Repeat Interval (in minutes):</label>
                                        <input
                                            type="number"
                                            className="form-control"
                                            id="inputRepeatInterval"
                                            value={repeatIntervalInMinutes}
                                            onChange={(e) => setRepeatIntervalInMinutes(e.target.value)}
                                            required
                                        />
                                    </>
                                )}
                                {isRepeating && (
                                    <>
                                        <label htmlFor="inputEndDateTime" className="form-label">End Date Time:</label>
                                        <input
                                            type="datetime-local"
                                            className="form-control"
                                            id="inputEndDateTime"
                                            value={endDateTime}
                                            onChange={(e) => setEndDateTime(e.target.value)}
                                            required
                                        />
                                    </>
                                )}
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={() => setShowCreateModal(false)}>Close</button>
                                <button type="submit" className="btn btn-primary">Submit</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            <div className="modal" tabIndex="-1" role="dialog" style={{ display: showEditModal ? "block" : "none" }}>
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <form onSubmit={handleEditSubmit}>
                            <div className="modal-header">
                                <h5 className="modal-title">Edit Slot</h5>
                            </div>
                            <div className="modal-body">
                                <div className="mb-3">
                                    <label htmlFor="editDateTime" className="form-label">Date Time:</label>
                                    <input
                                        type="datetime-local"
                                        className="form-control"
                                        id="editDateTime"
                                        value={editedSlot ? editedSlot.dateTime.slice(0, 16) : ''}
                                        onChange={(e) => setEditedSlot({ ...editedSlot, dateTime: e.target.value })}
                                        required
                                    />
                                </div>
                                <div className="mb-3">
                                    <label htmlFor="editEmail" className="form-label">Email:</label>
                                    <input
                                        type="email"
                                        className="form-control"
                                        id="editEmail"
                                        value={editedSlot ? editedSlot.email : ''}
                                        onChange={(e) => setEditedSlot({ ...editedSlot, email: e.target.value })}
                                    />
                                </div>
                                <div className="mb-3">
                                    <label htmlFor="editStatus" className="form-label">Status:</label>
                                    <select
                                        className="form-control"
                                        id="editStatus"
                                        value={editedSlot ? editedSlot.status : ''}
                                        onChange={(e) => setEditedSlot({ ...editedSlot, status: e.target.value })}
                                    >
                                        {statuses.map((status) => (
                                            <option key={status.value} value={status.value}>
                                                {status.name}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" onClick={() => setShowEditModal(false)}>Close</button>
                                <button type="submit" className="btn btn-primary">Save changes</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Home;
