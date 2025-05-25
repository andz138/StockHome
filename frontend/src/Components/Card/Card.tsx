import "./Card.css";

interface Props {
    companyName: string;
    ticker: string;
    price: number;
}
const Card : React.FC<Props> = ({ companyName, ticker, price }: Props) : JSX.Element => {
    return (
        <div className="card">
            <img src="https://fastly.picsum.photos/id/995/536/354.jpg?hmac=kARkIcQD-5FYzmRwd89uPn6yxoJvaCg43bkO-kABGGE"
                 alt="Image"/>
            <div className="details">
                <h2>{companyName} {ticker}</h2>
                <p>Price: ${price}</p>
            </div>            <p className="info">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Asperiores culpa ipsa non officia placeat quod repellat. Autem, corporis necessitatibus nesciunt nostrum praesentium recusandae saepe sequi sit tempore ullam veniam voluptatibus?</p>
        </div>    );
};
export default Card;