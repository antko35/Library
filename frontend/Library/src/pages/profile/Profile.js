import React, { useState, useEffect } from 'react';
import LoginForm from '../../сomponents/forms/LoginForm';
import RegisterForm from '../../сomponents/forms/RegisterForm';
import { Spin, Button } from 'antd'; 
import { useAuth } from '../../context/AuthContext'; // Import useAuth hook
import './Profile.css';

const Profile = () => {
  const { user, isAuthenticated, setIsAuthenticated, loading, logout,isRegistered, setIsRegistered } = useAuth();
  

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
          <Button type="primary" onClick={logout} >Logout</Button>
        </div>
      ) : (
        <div style={formStyle}>
          {isRegistered ? (
            <LoginForm setIsAuthenticated = {setIsAuthenticated} setIsRegistered={setIsRegistered} />
          ) : (
            <RegisterForm setIsRegistered={setIsRegistered} />
          )}
        </div>
      )}
    </div>
  );
};

export default Profile;
