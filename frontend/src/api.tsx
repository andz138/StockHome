import {CompanySearch} from "./company";
import axios from "axios";

interface SearchResponse {
    data: CompanySearch[];
}

// Function that calls external API
export const searchCompanies = async (query: string)=> {
    // Must use try-catch blocks as it is not uncommon for API calls to fail
    try {
        const data = await axios.get<SearchResponse>(
            `https://financialmodelingprep.com/api/v3/search?query=${query}&limit=10&exchange=NASDAQ&apikey=${import.meta.env.VITE_API_KEY}`
        );
        return data;
    } catch(error) {
        if(axios.isAxiosError(error)) {
            console.log("error message: ", error.message)
            return error.message;
        } else {
            console.log("unexpected error: ", error);
            return "An unexpected error has occurred."
        }
    }
}