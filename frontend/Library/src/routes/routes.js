// routes.js
import { Routes, Route } from 'react-router-dom';
import Home from '../pages/home/Home';
import Profile from '../pages/profile/Profile';
import AllBooks from '../pages/allBooks/AllBooks';

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />

      <Route path="/profile" element={<Profile />} />
      
      <Route path="/books" element={<AllBooks />} />
    </Routes>
  );
};

export default AppRoutes;