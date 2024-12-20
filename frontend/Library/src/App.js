import './App.css';
import { Layout } from 'antd';
import Header from './сomponents/header/Header';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import AppRoutes from './routes/routes'
import { ChakraProvider } from '@chakra-ui/react'
import { AuthProvider } from './context/AuthContext';


function App() {
  return (
    <ChakraProvider>
      <Router>
      <AuthProvider>
        <Layout style={{minHeight : '100vh'}} >
          <Header />
          <Layout.Content>
            <AppRoutes />
          </Layout.Content>
          <Layout.Footer style={{ textAlign: 'center' }}>
            BookStore ©2024
          </Layout.Footer>
        </Layout>
        </AuthProvider>
      </Router>
    </ChakraProvider>
  );
}

export default App;
