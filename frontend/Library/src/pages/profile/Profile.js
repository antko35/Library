import React, { useState, useEffect } from 'react';
import LoginForm from '../../сomponents/forms/LoginForm';
import RegisterForm from '../../сomponents/forms/RegisterForm';
import { Spin, Button } from 'antd'; // Импортируем компоненты Ant Design
import './Profile.css';

const Profile = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false); // Статус авторизации
  const [user, setUser] = useState({ name: '', email: '' }); // Данные пользователя
  const [isRegistered, setIsRegistered] = useState(true); // Статус регистрации
  const [loading, setLoading] = useState(true); // Состояние загрузки

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await fetch('https://localhost:7040/User/info', {
          method: 'GET',
          credentials: 'include',
        });

        if (response.ok) {
          const data = await response.json();
          setUser({ name: data.userName, email: data.email });
          setIsAuthenticated(true);
        } else {
          console.error('Failed to fetch user data');
        }
      } catch (error) {
        console.error('Error fetching user data:', error);
      } finally {
        setLoading(false); // Выключаем индикатор загрузки
      }
    };

    fetchUserData();
  }, []);

  const handleLogout = async () => {
    try {
      const response = await fetch('https://localhost:7040/User/logout', {
          method: 'POST',
          credentials: 'include',
      });

      if (response.ok) {
          localStorage.removeItem("jwt_cookie");
          setIsAuthenticated(false);
          setUser({ name: '', email: '' });
      } else {
          console.error('Logout failed');
      }
  } catch (error) {
      console.error('Error logging out:', error);
  }
  };

  const loginStyle = {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    minHeight: '100vh',
  };

  const formStyle = {
    backgroundColor: '#f0f0f0',
    padding: '20px',
    borderRadius: '10px',
    boxShadow: '0 4px 8px rgba(0,0,0,0.1)',
  };
  const loginFosmStyle ={
    backgroundColor: '#f0f0f0',
    padding: '40px',
    borderRadius: '12px',
    boxShadow: '0 6px 12px rgba(0,0,0,0.15)',
    fontSize: '18px',
  };

  return (
    <div style={loginStyle}>
      {loading ? (
        <Spin tip="Loading..." />
      ) : (isAuthenticated) ? (
        <div style={loginFosmStyle}>
          <p><strong>Name:</strong> {user.name}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <Button type="primary" onClick={handleLogout} >Logout</Button>
        </div>
      ) : (
        <div style={formStyle}>
          {isRegistered ? (
            <LoginForm setUser={setUser} setIsAuthenticated={setIsAuthenticated} setIsRegistered={setIsRegistered} />
          ) : (
            <RegisterForm setUser={setUser} setIsAuthenticated={setIsAuthenticated} setIsRegistered={setIsRegistered} />
          )}
        </div>
      )}
    </div>
  );
};

export default Profile;
