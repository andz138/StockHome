import './App.css'
import CardList from "./Components/CardList/CardList.tsx";
import Search from "./Components/Search/Search.tsx";
import {ChangeEvent, MouseEvent, SyntheticEvent, useState} from "react";

function App() {
    const [search, setSearch] = useState<string>("");
    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        setSearch(e.target.value);
        console.log(e);
    }
    
    const onClick = (e: SyntheticEvent) => {
        console.log(e);
    }
    
  return (
    <div className="App">
        <Search onClick={onClick} search={search} handleChange={handleChange}/>
        <CardList />
        
    </div>
  )
}

export default App
