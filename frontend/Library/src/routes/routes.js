// routes.js
import { Routes, Route } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Home from '../pages/home/Home';
import Profile from '../pages/profile/Profile';
import AllBooks from '../pages/allBooks/AllBooks';
import BookPage from '../pages/BookPage/bookPage';

const AppRoutes = () => {

  const { user } = useAuth();
  return (
    <Routes>
      <Route path="/" element={<Home />} />

      <Route path="/profile" element={<Profile />} />
      
      <Route path="/books" element={<AllBooks />} />
      <Route path="/book/:id" element={<BookPage isAdmin = {user.isAdmin} />} />
    </Routes>
  );
};

export default AppRoutes;