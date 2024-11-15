// AuthContext.js
import { createContext, useContext, useState, useEffect } from 'react';
import { message } from 'antd';
const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState({ name: '', email: '', isAdmin: false });       
  const [isAuthenticated, setIsAuthenticated] = useState(false);  
  const [isRegistered, setIsRegistered] = useState(true); 
  const [loading, setLoading] = useState(true); 
  const [errorMessage, setErrorMessage] = useState(null);
  const [emailError, setEmailError] = useState(null); 
  const [usernameError, setUsernameError] = useState(null); 
  

  const fetchUserData = async () => {
    try {
      const response = await fetch('https://localhost:7040/User/info', {
        method: 'GET',
        credentials: 'include', 
      });

      if (response.ok) {
        const data = await response.json();
        if(data.role === "Admin"){
          setUser({name: data.userName,email:data.email, isAdmin:true}); 
        }
        else{
          setUser({name: data.userName,email:data.email, isAdmin: false }); 
        }
        setIsAuthenticated(true );    
      }
    } catch (error) {
      console.error('Error fetching user data:', error);
    } finally {
      setLoading(false);          
    }
  };


  useEffect(() => {
    fetchUserData();
  }, []);


  const login = async (loginData)=>{
    try {
        setErrorMessage(null)
        const response = await fetch('https://localhost:7040/User/login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(loginData),
          credentials: 'include', // Для получения и отправки куков
        });
  
        if (response.ok) {
          await fetchUserData();
          setIsAuthenticated(true);
          message.success("seccessfully login");
        } else {
          var data = await response.json();
          console.error('Login failed:', data.error);
          setErrorMessage(data.error);
        }
      } catch (error) {
        console.error('Login failed:', error.message);
      }
  }

  const register = async (values) => {
    try {
      setEmailError(null);
      setUsernameError(null);
      const response = await fetch('https://localhost:7040/User/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: values.email,
          userName :values.username,
          password: values.password,
        }),
      });

      //const data = await response.json(); // Получаем текст ответа
      //console.log('Server response:', data);

      if (response.ok) {
        console.log('Registration successful');
        //setIsRegistered(true);
        const loginData = {
          email: values.email,
          password: values.password,
        };
        await login(loginData);
        message.success("seccessfully registered!!");
      } else {
        const data = await response.json(); // Получаем текст ответа
        console.log('Server response:', data);

        console.error('Registration failed:', data.error);

        if(data.error.includes('Usernames') && data.error.includes('Email')) {
          setEmailError("Username already in use");
          setUsernameError("Email already in use");
        }
        if(data.error.includes('Username')) {
          setUsernameError("Username already in use");
        }
        if(data.error.includes('Email')) {
          setEmailError("Email already in use");
        }
      }
    } catch (error) {
      console.error('Error during registration:', error);
    }
  };
  // Logs out the user and clears authentication state
  const logout = async () => {
    try {
      const response = await fetch('https://localhost:7040/User/logout', {
        method: 'POST',
        credentials: 'include',
      });
      if (response.ok) {
        setIsAuthenticated(false);      
        setUser({ name: '', email: '', isAdmin: false }); 
      } else {
        console.error('Logout failed');
      }
    } catch (error) {
      console.error('Error logging out:', error);
    }
  };

  return (
    <AuthContext.Provider value={{ user,setUser, isAuthenticated,setIsAuthenticated, isRegistered, setIsRegistered, loading , logout, login,register , errorMessage, emailError, usernameError}}>
      {children}
    </AuthContext.Provider>
  );
};

// Custom hook to use the AuthContext, making it easier to access authentication data
export const useAuth = () => useContext(AuthContext);
