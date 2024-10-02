import React, { useState,useEffect } from 'react';
import LoginForm from '../../сomponents/forms/LoginForm';
import './Profile.css';
import RegisterForm from '../../сomponents/forms/RegisterForm';

const Profile = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false); // Статус авторизации
  const [user, setUser] = useState({ name: '', email: '' }); // Данные пользователя
  const [isRegistered,SetIsRegistered ] = useState(true);
  const [loginData, setLoginData] = useState({ username: '', password: '' }); // Данные формы

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await fetch('https://localhost:7040/User/info', {
          method: 'GET',
          credentials: 'include', // Для отправки куков на сервер
        });

        if (response.ok) {
          const data = await response.json();
          setUser({ name: data.userName, email: data.email });
          setIsAuthenticated(true);
          // console.log(user.email)
        } else {
          console.error('Failed to fetch user data');
        }
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };

    fetchUserData();
  }, []);

  const login={
    display: 'flex',
    justifyContent : 'center',
    alignItems :  'center',
    minHeight: '100vh'
  }
  const formStyle ={
    backgroundColor: '#f0f0f0',
    padding: '20px',
    borderRadius: '10px',
    boxShadow: '0 4px 8px rgba(0,0,0,0.1)',
  }

  // Если пользователь авторизован, отображаем его данные
  if (isAuthenticated) {
    return (
      <div>
        <p><strong>Name:</strong> {user.name}</p>
        <p><strong>Email:</strong> {user.email}</p>
      </div>
    );
  }

  // Если пользователь не авторизован, отображаем форму входа
  return (
    <div style={login}>
      <div style={formStyle}>
      {isRegistered ? (
        <LoginForm setUser={setUser} setIsAuthenticated={setIsAuthenticated} SetIsRegistered={SetIsRegistered} />
      ) : (
        <RegisterForm setUser={setUser} setIsAuthenticated={setIsAuthenticated} SetIsRegistered={SetIsRegistered} />
      )}
      </div>
    </div>
  );
};


export default Profile;
