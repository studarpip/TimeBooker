import React from 'react';
import Home from './Components/Home';
import LoginForm from './Components/LoginForm';
import RegisterForm from './Components/RegisterForm';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';

function App() {
  return (
    <Router>
      <Switch>
        <Route path="/register" component={RegisterForm} />
        <Route path="/login" component={LoginForm} />
        <Route path="/" component={Home} />
      </Switch>
    </Router>
  );
}

export default App;
