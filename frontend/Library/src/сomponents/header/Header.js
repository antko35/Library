import React from 'react';
import { Menu} from 'antd';
import {Link} from 'react-router-dom'
import { Content } from 'antd/es/layout/layout';

const menuItems=[
        {key:'all-books',label: <Link to={"/"}>Your Books</Link>},
        {key:'my-books',label: <Link to={"/books"}>All Books</Link>},
        {key:'profile',label: <Link to={"/profile"}>Profile</Link>,style:{marginLeft :'auto'}}
    ];
function Header() {
    
  return (
    <Menu mode="horizontal" theme='dark' items={menuItems} />
  );
}

export default Header;